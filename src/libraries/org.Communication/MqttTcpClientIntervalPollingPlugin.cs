using CommunityToolkit.Mvvm.Messaging;
using org.Communication.Extensions;
using org.Models;
using org.Models.Messagings;
using org.Utils;
using org.Utils.Global;
using Serilog;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TouchSocket.Core;
using TouchSocket.Modbus;
using TouchSocket.Mqtt;

namespace org.Communication
{
    public class MqttTcpClientIntervalPollingPlugin : PluginBase, IMqttConnectedPlugin, IMqttReceivedPlugin, IMqttClosedPlugin
    {
        private string _key;
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

        public string Sn { get; private set; }
        private int POLLING_OFFSET => Sn?.LastOrDefault() ?? 0;
        private int POLLING_SLEEPMS = ConfigContext.GenericConfig?.MqttConfig?.PollingInterval ?? 20000;
        private ModbusDataStorage _storage;
        /// <summary>
        /// key: _key + "_"+functionCode+"_"+startAddress
        /// value: 
        /// </summary>
        private Dictionary<string, PointMapRangMapping> _registerMappings;

        private int ProcessCore(IMqttClient client)
        {

            return 0;
        }

        private void PollingCore(object clientObj)
        {
            int spanMs = 0;
            var client = clientObj as IMqttClient;
            var polling = POLLING_SLEEPMS + POLLING_OFFSET;

            // 从空闲转运行中
            while (GetLock())
            {
                try
                {
                    if (ConnectState(client))
                    {
                        spanMs = ProcessCore(client);
                        Debug.WriteLine($"mtcipp.Polling {POLLING_NO++}: span " + spanMs);
                    }
                    else
                    {
                        spanMs = 0;
                    }
                }
                catch (Exception ex)
                {
                    LogFactory.Error(ex,
                        $"MqttTcpClientIntervalPollingPlugin.PollingCore {Sn}：" +
                        $"{Environment.CurrentManagedThreadId} Error");
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
                $"MqttTcpClientIntervalPollingPlugin.TryRelease: {Sn}");
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

        static ReadOnlyMemory<ushort> BUFFER_EMPTY = new ushort[125].AsMemory();
        private int EmptyReadData(ModbusDataStorage storage, ushort lengthPerGroup = 125, bool forceSend = false)
        {
            var bucks = _registerMappings.Select(
                s => new KeyValuePair<string, ReadOnlyMemory<ushort>>(s.Key, BUFFER_EMPTY));

            storage.ReceivingBuck([.. bucks], forceSend, token:_key);
            return -1000;
        }

        private void InitialRegisterMaps()
        {
            _registerMappings = new Dictionary<string, PointMapRangMapping>();
            var currentSolution = EmsSolutionContext.Current;

            var mappings = currentSolution?.Settings?.PointMapMappings;
            if (mappings?.Any() != true)
            {
                return;
            }

            foreach (var mapping in mappings)
            {
                var key = _key + "_" + (int)mapping.FunctionCode + "_" + mapping.StartAddress;
                _registerMappings[key] = mapping;
            }

        }

        public async Task OnMqttConnected(IMqttSession client, MqttConnectedEventArgs e)
        {
            var obj = CommunicateAdapterPool.ModbusMasterPool
                .FirstOrDefault(f => (f.Value as MqttTcpClientObject)?.Instance == client);
            if (obj.Value is null)
            {
                return;
            }

            var mqttClient = obj.Value as MqttTcpClientObject;

            _key = obj.Key;
            Sn = mqttClient.Sn;
            InitialRegisterMaps();
            if (CommunicateAdapterPool.StoragePool
                .TryGetValue(mqttClient.OriginConfig.GetKey(), out var storage))
            {
                _storage = storage;
            }

            LogFactory.Info("MqttClinetIntervalPollingPlugin.OnMqttConnected");

            ReInitializeThread(client);
        }

        public async Task OnMqttClosed(IMqttSession client, MqttClosedEventArgs e)
        {
            LogFactory.Info($"MqttTcpClinetIntervalPollingPlugin.OnMqttClosed: {Sn}");
            TryRelease();

            var storage = CommunicateAdapterPool.StoragePool[_key];

            await Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(t =>
            {
                storage?.ClearBuffers();
                EmptyReadData(storage, forceSend: true);
            });
        }

        private async Task Topic04Handler(MqttArrivedMessage arrivedMessage, string sn)
        {
            const int index_Count = 42;
            var buff = new byte[arrivedMessage?.Payload.Length ?? 0];
            if (buff.Length < index_Count)
            {
                return;
            }

            arrivedMessage.Payload.CopyTo(buff);
            WriteLog(buff);
            var count = buff[index_Count];
            buff = [.. buff.Skip(index_Count + 1)];
            var list = SplitPayload(sn, arrivedMessage, buff, functionCode: 4, count);


            _storage.ReceivingBuck(list, token: _key);

        }

        private async Task Topic05Handler(MqttArrivedMessage arrivedMessage, string sn)
        {
            var buff = new byte[arrivedMessage?.Payload.Length ?? 0];
            if (buff.Length < 6)
            {
                return;
            }

            arrivedMessage.Payload.CopyTo(buff);
            WriteLog(buff);

            buff = [.. buff.Skip(36)];
            var count = 1;
            var list = SplitPayload(sn, arrivedMessage, buff, functionCode: 3, count);


            _storage.ReceivingBuck(list, token: _key);

        }

        private async Task Topic06Handler(MqttArrivedMessage arrivedMessage, string sn)
        {
            var buff = new byte[arrivedMessage?.Payload.Length ?? 0];
            if (buff.Length < 40)
            {
                return;
            }

            arrivedMessage.Payload.CopyTo(buff);
            WriteLog(buff);

            buff = [.. buff.Skip(36)];
            var start = (buff[0] << 8) + buff[1];
            var result = new RawMqttMessage()
            {
                FunctionCode = 3,
                StartAddress = start,
                EndAddress = start,
                Topic = arrivedMessage.TopicName,
                QosLevel = (int)arrivedMessage.QosLevel,
                Sn = sn,
                Buffers =
                [
                    new($"{_key}_3_{start}~{start}",buff.AsMemory())
                ]
            };

            WeakReferenceMessenger.Default.Send(result, token: _key);
        }

        private async Task Topic16Handler(MqttArrivedMessage arrivedMessage, string sn)
        {
            var buff = new byte[arrivedMessage?.Payload.Length ?? 0];
            if (buff.Length < 42)
            {
                return;
            }

            arrivedMessage.Payload.CopyTo(buff);
            WriteLog(buff);

            buff = [.. buff.Skip(36)];
            var start = (buff[0] << 8) + buff[1];
            var end = (buff[2] << 8) + buff[3];
            var result = new RawMqttMessage()
            {
                FunctionCode = 3,
                StartAddress = start,
                EndAddress = end,
                Topic = arrivedMessage.TopicName,
                QosLevel = (int)arrivedMessage.QosLevel,
                Sn = sn,
                Buffers =
                [
                    new($"{_key}_3_{start}~{end}",buff.AsMemory())
                ]
            };

            WeakReferenceMessenger.Default.Send(result, _key);
        }

        /// <summary>
        /// 服务器请求处理
        /// "0x04/{sn}",采集器主动上传输入寄存器
        /// "0x05/result/{sn}",网络服务器对EMS多个保持寄存器的读取
        /// "0x06/result/{sn}",网络服务器对EMS单个保持寄存器的设置
        /// "0x10/result/{sn}",网络服务器对EMS多个保持寄存器的设置
        /// </summary>
        /// <param name="arrivedMessage"></param>
        /// <returns></returns>
        private async Task MessageHubAsync(MqttArrivedMessage arrivedMessage)
        {
            Debug.WriteLine($"{arrivedMessage.TopicName}: {arrivedMessage.Payload.ToLogString()}");
            try
            {
                var index_LastSplash = arrivedMessage.TopicName.LastIndexOf('/');
                if (index_LastSplash < 0) { return; }

                var sn = arrivedMessage.TopicName[(index_LastSplash + 1)..];
                var head = arrivedMessage.TopicName[..index_LastSplash];
                switch (head)
                {
                    case "0x04":
                        await Topic04Handler(arrivedMessage, sn);
                        break;
                    case "0x05/result":
                        await Topic05Handler(arrivedMessage, sn);
                        break;
                    case "0x06/result":
                        await Topic06Handler(arrivedMessage, sn);
                        break;
                    case "0x10/result":
                        await Topic16Handler(arrivedMessage, sn);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"MqttTcpClinetIntervalPollingPlugin.MessageHubAsync Error: {arrivedMessage.TopicName}");
            }
        }

        public async Task OnMqttReceived(IMqttSession client, MqttReceivedEventArgs e)
        {
            await MessageHubAsync(e.MqttMessage);

            await e.InvokeNext();
        }

        private List<KeyValuePair<string, ReadOnlyMemory<ushort>>> SplitPayload(
            string sn, MqttArrivedMessage arrivedMessage, byte[] buff, int functionCode, int count)
        {
            int no = 0;
            var rawList = new List<KeyValuePair<string, ReadOnlyMemory<byte>>>();
            var list = new List<KeyValuePair<string, ReadOnlyMemory<ushort>>>();
            for (var index = 0; index < buff.Length && no < count; no++)
            {
                try
                {
                    var start = (buff[index] << 8) + buff[index + 1];
                    var end = (buff[index + 2] << 8) + buff[index + 3];
                    var dataCount = ((end - start + 1) * 2 + 4);
                    if (start >= end || dataCount > 254)
                    {
                        break;
                    }

                    var data = buff.Skip(index + 4 * no + 4).Take(250).ToArray();
                    rawList.Add(new KeyValuePair<string, ReadOnlyMemory<byte>>(
                        $"{_key}_{functionCode}_{start}~{end}", data.AsMemory()));
                    if (data.Length < 250)
                    {
                        var newData = new byte[250];
                        Array.Copy(data, newData, data.Length);
                        list.Add(new KeyValuePair<string, ReadOnlyMemory<ushort>>(
                            $"{_key}_{functionCode}_{start}", newData.GetBigEndianU16s()));
                        continue;
                    }


                    list.Add(new KeyValuePair<string, ReadOnlyMemory<ushort>>(
                        $"{_key}_{functionCode}_{start}", data.GetBigEndianU16s()));
                    index += dataCount;
                }
                catch (Exception ex)
                {

                }
            }

            WeakReferenceMessenger.Default.Send(new RawMqttMessage()
            {
                Sn = sn,
                Buffers = rawList,
                FunctionCode = functionCode,
                Topic = arrivedMessage.TopicName,
                QosLevel = (int)arrivedMessage.QosLevel
            }, _key);

            return list;
        }

        private void WriteLog(byte[] buff)
        {
#if DEBUG
            for (var i = 0; i < buff.Length; i += 16)
            {
                try
                {
                    var data = buff[i..(buff.Length > (i + 15) ? (i + 16) : (i + buff.Length % 16))];
                    StringBuilder sb = new StringBuilder();
                    for (var j = 0; j < data.Length; j += 2)
                    {
                        sb.Append($"{data[j]:x2}" + (data.Length > (j + 1) ? data[j + 1].ToString("x2") : "") + " ");
                    }

                    Debug.WriteLine(sb.ToString());
                }
                catch (Exception ex)
                {

                }
            }
#endif
        }
    }
}
