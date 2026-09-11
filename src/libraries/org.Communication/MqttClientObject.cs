using org.Models;
using org.Utils.Global;
using Microsoft.Extensions.Primitives;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TouchSocket.Core;
using TouchSocket.Mqtt;
using TouchSocket.Sockets;

namespace org.Communication
{
    /// <summary>
    /// mqtt客户端
    /// 主要职责：
    /// 1、定时转发本地ems点表至服务器
    /// 2、接收服务器请求并下发至ems
    /// 3、接受服务器请求调整采集器参数
    /// 
    /// </summary>
    public class MqttClientObject
    {
        public DateTime HealthyReplyTime { get; set; }
        private ReadOnlyMemory<byte> _healthyFrame
            = new ReadOnlyMemory<byte>(new byte[] { 0x5A, 0xA5, 0, 0, 0x3, 0x16 });
        public bool Online => Instance?.Online ?? false;
        public MqttTcpClient Instance { get; private set; }
        public MqttConnectOptions Options { get; private set; }
        public MqttEnvironmentConfig OriginConfig { get; private set; }

        public List<SubscribeRequest> Topics { get; private set; }

        public string Sn { get; private set; }
        /// <summary>
        /// modbusmaster key
        /// </summary>
        public string ModbusKey { get; private set; }

        public string EmsDeviceKey { get; private set; }

        public string ClientId { get; private set; }

        public MqttClientObject(string sn, string key)
        {
            Sn = sn;
            ModbusKey = key;
            HealthyReplyTime = DateTime.Now;
        }

        public async Task InitializeAsync(string key, MqttEnvironmentConfig options, List<string> defTopics)
        {
            Instance = new MqttTcpClient();

            //配置
            OriginConfig = options;
            var config = new TouchSocketConfig();
            config.SetRemoteIPHost($"{options.Protocol}{options.Ip}:{options.Port}");


            //配置Mqtt连接参数
            #region Mqtt客户端配置连接选项
            config.SetMqttConnectOptions(opt =>
            {
                opt.ClientId = ClientId = Guid.NewGuid().ToString("N");
                opt.UserName = options.Account;
                opt.Password = options.Password;
                opt.ProtocolName = "MQTT";

                opt.Version = (MqttProtocolVersion)options.MqttVersion;
                opt.KeepAlive = 200;//此处的KeepAlive仅仅会将数值传递给服务器，客户端并不会发送心跳数据，如有需要请启用mqtt断线重连兼心跳插件
                opt.CleanSession = true;
            });
            #endregion
            #region Mqtt客户端自动重连配置
            config.ConfigurePlugins(a =>
            {
                a.Add<MqttClientIntervalPollingPlugin>();

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
            Options = config.GetMqttConnectOptions();
            SetTopics(defTopics);
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
                    await InitializeAsync("", OriginConfig, null);
                }

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
            await Instance?.CloseAsync($"{Options?.ClientId} going offline.");
        }

        public async Task CloseAsync()
        {
            await DisconnectAsync();
            Instance?.Dispose();
            Instance = null;
        }

        public void SetTopics(List<string> topics,QosLevel qosLevel= QosLevel.AtMostOnce)
        {
            if (topics?.Any() == true)
            {
                Topics = [.. topics.Select(s => new SubscribeRequest(s.Replace("{sn}", Sn), qosLevel))];
            }
        }

        public async Task SubscribeAsync(string topic, QosLevel qosLevel)
        {
            if (Topics.FirstOrDefault(f => f.Topic == topic) is SubscribeRequest request)
            {
                return;
            }

            request = new SubscribeRequest(topic, qosLevel);
            var message = new MqttSubscribeMessage(request);
            await Instance?.SubscribeAsync(message);
            Topics.Add(request);
        }
    }
}
