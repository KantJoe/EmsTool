using org.Communication.MqttTopics;
using org.Utils;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Modbus;
using TouchSocket.Mqtt;
using TouchSocket.SerialPorts;

namespace org.Communication
{
    public class MqttClientIntervalPollingPlugin : PluginBase, IMqttReceivedPlugin, IMqttConnectedPlugin, IMqttClosedPlugin
    {
        private long POLLING_NO = 0;
        private Thread _pollingThread;
        /// <summary>
        /// <=0 : 无效
        /// = 1 ：运行中
        /// = 2 ：空闲
        /// </summary>
        private int _pollingState_Lock;
        private const int POLLINGSTATE_LOCK_RUNNING = 1;
        private const int POLLINGSTATE_LOCK_IDLE = 2;
        public string ModbusKey { get; private set; }
        public string Sn { get; private set; }
        private int POLLING_OFFSET => Sn?.LastOrDefault() ?? 0;
        private int POLLING_SLEEPMS = ConfigContext.GenericConfig?.MqttConfig
            ?.PollingInterval ?? 20000;

        private void PollingCore(object clientObj)
        {
            int spanMs = 0;
            var client = clientObj as IMqttClient;
            var polling = POLLING_SLEEPMS + POLLING_OFFSET;

            InitializeAllState();

            // 从空闲转运行中
            while (GetLock())
            {
                try
                {
                    if (ConnectState(client))
                    {
                        spanMs = ProcessCore(client);
                        Debug.WriteLine($"mcipp.Polling {POLLING_NO++}: span " + spanMs);
                    }
                    else
                    {
                        spanMs = -20000;
                    }
                }
                catch (Exception ex)
                {
                    LogFactory.Error(ex,
                        $"MqttClinetIntervalPollingPlugin.PollingCore {Sn}：" +
                        $"{Thread.CurrentThread.ManagedThreadId} Error");
                }
                finally
                {
                    // 从运行中转空闲
                    ReleaseLock();
                    Thread.Sleep(GetSleepMs(polling - spanMs, polling));
                    spanMs = 0;
                }
            }

            _pollingThread = null;
        }

        private int ProcessCore(IMqttClient client)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Send0x02Info(client);
            Send0x03Info(client);
            Send0x04Info(client);

            return (int)sw.Elapsed.TotalMilliseconds;
        }

        private void Send0x04Info(IMqttClient client)
        {
            var topic = new SubmitCollector04Topic(Sn);
            topic.ComposeData(ModbusKey);
            topic.PublishAsync(ModbusKey, client).ConfigureFalseAwait()
                .GetAwaiter().GetResult();
        }

        private void Send0x03Info(IMqttClient client)
        {
            var topic = new SubmitCollector03Topic(Sn);
            topic.ComposeData(ModbusKey);
            topic.PublishAsync(ModbusKey, client).ConfigureFalseAwait()
                .GetAwaiter().GetResult();
        }

        private bool STATE_SEND0X02;
        private void Send0x02Info(IMqttClient client)
        {
            if (STATE_SEND0X02) return;


            var topic = new SubmitCollector02Topic();
            topic.ComposeData(ModbusKey, $"{ModbusKey}_3_0", $"{ModbusKey}_3_375");
            topic.PublishAsync(ModbusKey, client).ConfigureFalseAwait()
                .GetAwaiter().GetResult();
        }

        private void InitializeAllState()
        {
            STATE_SEND0X02 = false;
        }

        private bool GetLock()
        {
            return Interlocked.CompareExchange(ref _pollingState_Lock,
                POLLINGSTATE_LOCK_RUNNING, POLLINGSTATE_LOCK_IDLE)
                == POLLINGSTATE_LOCK_IDLE;
        }

        private bool ReleaseLock()
        {
            return Interlocked.CompareExchange(ref _pollingState_Lock,
                POLLINGSTATE_LOCK_IDLE, POLLINGSTATE_LOCK_RUNNING)
                == POLLINGSTATE_LOCK_RUNNING;
        }

        private int GetSleepMs(int calcMs, int defMs)
        {
            return Math.Max(1, Math.Min(calcMs, defMs));
        }

        private void TryRelease()
        {
            Interlocked.Exchange(ref _pollingState_Lock, 0);

            LogFactory.Info(
                $"MqttClinetIntervalPollingPlugin.TryRelease: {Sn}");
        }

        private void ReInitializeThread(IMqttSession client)
        {

            TryRelease();

            _pollingState_Lock = 2;
            _pollingThread = new Thread(new ParameterizedThreadStart(Polling));
            _pollingThread.IsBackground = true;
            _pollingThread.Start(client);
        }

        private void Polling(object obj)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(POLLING_SLEEPMS + POLLING_OFFSET));
            PollingCore(obj);
        }

        private bool ConnectState(IMqttClient client)
        {
            return client?.Online ?? false;
        }

        public async Task OnMqttReceived(IMqttSession client, MqttReceivedEventArgs e)
        {
            await MessageHubAsync(e.MqttMessage);


            await e.InvokeNext();
        }

        /// <summary>
        /// 服务器请求处理
        /// "0x05/{sn}",网络服务器对EMS多个保持寄存器的读取
        /// "0x06/{sn}",网络服务器对EMS单个寄存器的设置
        /// "0x10/{sn}",网络服务器对EMS多个寄存器的设置
        /// "0x16/{sn}",心跳包
        /// "0x18/{sn}",设置数据采集器属性
        /// "0x19/{sn}",查询数据采集器属性
        /// "0x51/{sn}",用于平台主动查询0x04指令的历史数据
        /// "0x80/{sn}",网络服务器对EMS多个输入寄存器的读取 
        /// "0xA0/{sn}" 用于平台主动查询PCS、DCDC、STS等数据
        /// </summary>
        /// <param name="arrivedMessage"></param>
        /// <returns></returns>
        private async Task MessageHubAsync(MqttArrivedMessage arrivedMessage)
        {
            Debug.WriteLine($"{arrivedMessage.TopicName}: {arrivedMessage.Payload.ToLogString()}");
            try
            {
                var head = arrivedMessage.TopicName[..4];
                switch (head)
                {
                    case "0x02":
                        await Collector02InfoAsync(arrivedMessage);
                        break;
                    case "0x03":
                        await Collector03InfoAsync(arrivedMessage);
                        break;
                    case "0x16":
                        await HealthyCheckAsync(arrivedMessage);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"MqttClinetIntervalPollingPlugin.MessageHubAsync Error: {arrivedMessage.TopicName}");
            }
        }

        private async Task Collector03InfoAsync(MqttArrivedMessage arrivedMessage)
        {
            var lastIndex = arrivedMessage.TopicName.LastIndexOf('/') + 1;
            if (lastIndex <= 0)
            {
                return;
            }

            Sn = arrivedMessage.TopicName[lastIndex..];
            if (!CommunicateAdapterPool.MqttClientPool
                .TryGetValue(Sn, out MqttClientObject client))
            {
                Sn = string.Empty;
                return;
            }

            var response = new MqttStateResultMessage(arrivedMessage.Payload);
            Debug.WriteLine($"Mqtt Response {response.Code:x2}:{response.State}");
        }

        private async Task Collector02InfoAsync(MqttArrivedMessage arrivedMessage)
        {
            var lastIndex = arrivedMessage.TopicName.LastIndexOf('/') + 1;
            if (lastIndex <= 0)
            {
                return;
            }

            Sn = arrivedMessage.TopicName[lastIndex..];
            if (!CommunicateAdapterPool.MqttClientPool
                .TryGetValue(Sn, out MqttClientObject client))
            {
                Sn = string.Empty;
                return;
            }

            var response = new MqttStateResultMessage(arrivedMessage.Payload);
            Debug.WriteLine($"Mqtt Response {response.Code:x2}:{response.State,2}");
            STATE_SEND0X02 = response.State == 0;
        }

        private async Task HealthyCheckAsync(MqttArrivedMessage arrivedMessage)
        {
            var lastIndex = arrivedMessage.TopicName.LastIndexOf('/') + 1;
            if (lastIndex <= 0)
            {
                return;
            }

            Sn = arrivedMessage.TopicName[lastIndex..];
            if (!CommunicateAdapterPool.MqttClientPool
                .TryGetValue(Sn, out MqttClientObject client))
            {
                Sn = string.Empty;
                return;
            }

            client.HealthyReplyTime = DateTime.Now;
        }

        protected override void Loaded(IPluginManager pluginManager)
        {
            LogFactory.Info("MqttClinetIntervalPollingPlugin.Loaded");
        }

        public async Task OnMqttConnected(IMqttSession client, MqttConnectedEventArgs e)
        {
            var obj = CommunicateAdapterPool.MqttClientPool
                .FirstOrDefault(f => f.Value.Instance == client);
            if (obj.Value is null)
            {
                return;
            }

            Sn = obj.Value.Sn;
            ModbusKey = obj.Value.ModbusKey;
            LogFactory.Info("MqttClinetIntervalPollingPlugin.OnMqttConnected");

            ReInitializeThread(client);
        }

        protected override void Unloaded(IPluginManager pluginManager)
        {
            LogFactory.Info(
                $"MqttClinetIntervalPollingPlugin.UnLoaded: {Sn}");

        }

        public async Task OnMqttClosed(IMqttSession client, MqttClosedEventArgs e)
        {
            LogFactory.Info($"MqttClinetIntervalPollingPlugin.OnMqttClosed: {Sn}");
            TryRelease();
        }
    }
}
