using org.Models;
using org.Utils;
using org.Utils.Global;
using OpenTK.Graphics.ES11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Mqtt;
using TouchSocket.Sockets;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace org.Communication
{
    public class MqttTcpClientObject : IConnectClient, IRegisterOperation
    {
        public EmsConnectionOption Options => OriginConfig;
        private ReadOnlyMemory<byte> _healthyFrame
            = new([0x5A, 0xA5, 0, 0, 0x3, 0x16]);
        public DateTime HealthyReplyTime { get; set; }
        public bool Online => Instance?.Online ?? false;
        public string Key => $"{OriginConfig?.Ip}_{OriginConfig?.Port}";
        public MqttTcpClient Instance { get; private set; }
        public MqttConnectOptions InnerOptions { get; private set; }
        public MqttEnvironmentConfig OriginConfig { get; set; }

        public string Sn { get; set; }
        public List<SubscribeRequest> Topics { get; private set; }

        public MqttTcpClientObject()
        {
        }

        public MqttTcpClientObject(string sn)
        {
            Sn = sn;
        }

        public async Task InitializeAsync(EmsConnectionOption mqttOptions)
        {
            Instance = new MqttTcpClient();

            //配置
            OriginConfig = (MqttEnvironmentConfig)mqttOptions;
            var config = new TouchSocketConfig();
            config.SetRemoteIPHost($"{OriginConfig.Protocol}{OriginConfig.Ip}:{OriginConfig.Port}");

            //配置Mqtt连接参数
            #region Mqtt客户端配置连接选项
            config.SetMqttConnectOptions(opt =>
            {
                opt.ClientId = Guid.NewGuid().ToString("N");
                opt.UserName = OriginConfig.Account;
                opt.Password = OriginConfig.Password;
                opt.ProtocolName = "MQTT";

                opt.Version = (MqttProtocolVersion)OriginConfig.MqttVersion;
                opt.KeepAlive = 200;//此处的KeepAlive仅仅会将数值传递给服务器，客户端并不会发送心跳数据，如有需要请启用mqtt断线重连兼心跳插件
                opt.CleanSession = true;
            });
            #endregion
            #region Mqtt客户端自动重连配置
            config.ConfigurePlugins(a =>
            {
                a.Add<MqttTcpClientIntervalPollingPlugin>();
                //此处如果是Tcp协议的Mqtt客户端，则使用MqttTcpClient
                //如果是WebSocket协议的Mqtt客户端，则使用MqttWebSocketClient
                a.UseReconnection<MqttTcpClient>(opt =>
                {
                    opt.Strategy = ReconnectionStrategy.Custom;
                    opt.MaxRetryCount = 3;
                    opt.BaseInterval = TimeSpan.FromSeconds(5);
                    opt.PollingInterval = TimeSpan.FromSeconds(10);//轮询检验间隔为180秒
                    opt.CheckAction = HealthyCheckAsync;
                    opt.ConnectAction = ReconnectActionAsync;
                    opt.OnFailed = (client, count, ex) =>
                    {

                    };

                });
            });
            #endregion

            await Instance.SetupAsync(config);
            InnerOptions = config.GetMqttConnectOptions();
            var mqttTopics = ConfigContext.GenericConfig.MqttConfig.DefaultTopics;
            SetTopics(mqttTopics);
        }

        private async Task<ConnectionCheckResult> HealthyCheckAsync(MqttTcpClient client)
        {
            Debug.WriteLine("Mqtt Healthy Check " + DateTime.Now);
            try
            {
                if (client?.Online != true)
                {
                    Debug.WriteLine("Mqtt Healthy Check: Not online " + DateTime.Now);
                    return ConnectionCheckResult.Dead;
                }

                var time = (DateTime.Now - HealthyReplyTime).TotalSeconds;
                if (time >= 20f)
                {
                    var source = new CancellationTokenSource();

                    source.CancelAfter(TimeSpan.FromSeconds(5));
                    await client?.PublishAsync(
                        new MqttPublishMessage($"0x16/{Sn}", false, QosLevel.AtMostOnce, _healthyFrame),
                        source.Token);
                    if (source.IsCancellationRequested)
                    {
                        Debug.WriteLine("Mqtt Healthy Check: timeout " + DateTime.Now);
                        return ConnectionCheckResult.Dead;
                    }
                }

                Debug.WriteLine($"Mqtt Healthy Check: alive {time}," + DateTime.Now);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Mqtt Healthy Check: Error " + DateTime.Now);
                LogFactory.Error(ex, "MqttClientObject.HealthyCheckAsync");
                return ConnectionCheckResult.Dead;
            }

            return ConnectionCheckResult.Alive;
        }

        private async Task ReconnectActionAsync(MqttTcpClient client, CancellationToken token)
        {
            try
            {
                await client?.ConnectAsync(token);
                if (Topics?.Any() == true)
                {
                    var message = await client?.SubscribeAsync(
                        new MqttSubscribeMessage(Topics.ToArray()));
                }
                Debug.WriteLine($"Mqtt ReconnectActionAsync succeed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Mqtt ReconnectActionAsync Error");
                LogFactory.Error(ex, "MqttClientObject.ReconnectActionAsync");
            }
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (Instance is null)
                {
                    await InitializeAsync(OriginConfig);
                }

                Instance.SetPauseReconnection(false);
                await Instance?.ConnectAsync();
                if (Topics?.Any() == true)
                {
                    var message = await Instance?.SubscribeAsync(
                        new MqttSubscribeMessage([.. Topics]));
                }

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "MqttClientObject.ConnectAsync");
            }

            return Online;
        }

        public async Task DisconnectAsync()
        {
            var result = await Instance?.CloseAsync();
            Instance.SetPauseReconnection(true);
        }

        public async Task CloseAsync()
        {
            await DisconnectAsync();
            Instance?.Dispose();
        }

        public void SetTopics(List<string> topics, QosLevel qosLevel = QosLevel.AtMostOnce)
        {
            if (topics?.Any() == true)
            {
                Topics = [.. topics.Select(
                s => new SubscribeRequest(s.Replace("{sn}", Sn), qosLevel))];
            }
        }

        public async Task WriteHoldingRegisters(byte slaveId, ushort startingAddress, ReadOnlyMemory<byte> bytes)
        {
            if (Instance?.Online != true)
            {
                return;
            }

            var count = bytes.Length / 2;
            var endingAddress = startingAddress + count - 1;
            var topic = $"0x06/{Sn}";
            var code = count > 1 ? 0x10 : 6;
            var payload = new byte[] { 0x5a, 0xa5, 00, 00, 00, (byte)code,
                (byte)(startingAddress>>8),(byte)(startingAddress&0xFF),
            };

            if (count > 1)
            {
                topic = $"0x10/{Sn}";
                payload =
                [
                    .. payload,
                    .. new byte[] {
                    (byte)(endingAddress>>8),(byte)(endingAddress&0xFF) },
                ];
            }

            byte[] memory = [.. payload, .. bytes.ToArray()];
            memory[3] = (byte)((memory.Length - 3) >> 8);
            memory[4] = (byte)((memory.Length - 3) & 0xFF);
            await Instance.PublishAsync(new MqttPublishMessage(
                topic, false, QosLevel.AtMostOnce, memory.AsMemory()));
        }

        public async Task<ReadOnlyMemory<ushort>> ReadHoldingRegisters(byte slaveId, ushort startingAddress, ushort count)
        {
            return Array.Empty<ushort>();
        }

        public async Task<ReadOnlyMemory<ushort>> ReadInputRegisters(byte slaveId, ushort startingAddress, ushort count)
        {
            return Array.Empty<ushort>();
        }
    }
}
