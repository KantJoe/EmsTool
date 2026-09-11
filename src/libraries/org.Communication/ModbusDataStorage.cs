using CommunityToolkit.Mvvm.Messaging;
using org.Communication.Extensions;
using org.Models;
using org.Ui;
using org.Ui.MultiLanguage;
using org.Utils;
using org.Utils.Global;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TouchSocket.Modbus;

namespace org.Communication
{
    public class ModbusDataStorage
    {
        private int _cacheCount;
        private ConcurrentDictionary<string, ILogger> _loggerDict;
        /// <summary>
        /// 分组缓存
        /// key: modbuskey_functionCode_startAddress
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentBoundQueue<ReadOnlyMemory<ushort>>> PatitionBuffer { get; private set; }
        /// <summary>
        /// 分组缓存
        /// key: modbuskey_functionCode_startAddress
        /// </summary>
        private ConcurrentDictionary<string, DateTime> _sendLogDict;

        /// <summary>
        /// 允许执行告警
        /// </summary>
        public bool AllowAlert { get; set; }
        /// <summary>
        /// 告警点表
        /// key: functionCode_address
        /// </summary>
        public ConcurrentDictionary<string, EmsDevicePointAlert> AlertPoints { get; private set; }

        /// <summary>
        /// 告警历史
        /// key: functionCode_address
        /// </summary>
        public ConcurrentDictionary<string, PointAlertLog> AlertLogs { get; private set; }

        public string ModbusKey { get; private set; }

        public ModbusDataStorage(string modbusKey, int cacheCount = 10)
        {
            ModbusKey = modbusKey;
            _cacheCount = cacheCount;
            _loggerDict = new ConcurrentDictionary<string, ILogger>();
            _sendLogDict = new ConcurrentDictionary<string, DateTime>();
            PatitionBuffer = new ConcurrentDictionary<string, ConcurrentBoundQueue<ReadOnlyMemory<ushort>>>();

            AlertPoints = new ConcurrentDictionary<string, EmsDevicePointAlert>();
            var solution = EmsSolutionContext.Current;
            var device = solution?.DeviceTopologies
                ?.FirstOrDefault(f => f?.ConnectionOptions?.GetKey() == modbusKey);
            AppendAlerts(device?.AlertPoints);

            AlertLogs = new ConcurrentDictionary<string, PointAlertLog>();
        }

        public void AppendAlerts(List<EmsDevicePointAlert> points)
        {
            if (points?.Any() != true)
            {
                return;
            }

            foreach (var point in points)
            {
                AlertPoints[$"{(int)point.FunctionCode}_{point.StartAddress}"] = point;
            }
        }

        public static ReadOnlyMemory<ushort> Empty => new ReadOnlyMemory<ushort>(new ushort[125]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucks"></param>
        /// <param name="forceSend"></param>
        public void ReceivingBuck(List<KeyValuePair<string, ReadOnlyMemory<ushort>>> bucks, bool forceSend = false, string token = null)
        {
            if (bucks?.Any() != true)
            {
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            //Parallel.ForEach(bucks,
            //    new ParallelOptions
            //    {
            //        MaxDegreeOfParallelism = Math.Max(4, Environment.ProcessorCount - 4)
            //    },
            bucks.ForEach(item =>
            {
                var uniqueKey = item.Key;
                var chars = uniqueKey.Split("_");
                var functionCode = (ModbusFunctionCode)int.Parse(chars[^2]);
                var startAddr = int.Parse(chars.Last());
                if (!PatitionBuffer.ContainsKey(uniqueKey))
                {
                    _sendLogDict[uniqueKey] = DateTime.Now;
                    PatitionBuffer[uniqueKey] = new ConcurrentBoundQueue<ReadOnlyMemory<ushort>>(_cacheCount);
                    _loggerDict[uniqueKey] = LogFactory.CreateLogger(
                        uniqueKey, FilePathConst.CsvExt,
                        "[{Timestamp:HH:mm:ss.fff}] {Message}{NewLine}");
                }

                NormalAlertProcess(uniqueKey, functionCode, startAddr, item.Value);
                TrySendMesage(uniqueKey, functionCode, startAddr, item.Value, forceSend, token);
                WriteLog(uniqueKey, item.Value);
            });
            sw.Stop();
        }

        private void NormalAlertProcess(string uniqueKey, ModbusFunctionCode functionCode, int startAddr,
            ReadOnlyMemory<ushort> memory)
        {
            if (!AllowAlert)
            {
                return;
            }

            var items = AlertPoints.Where(a => uniqueKey == $"{ModbusKey}_{(int)a.Value.FunctionCode}_{a.Value.StartAddress / 125 * 125}")
                ?.OrderBy(o => o.Value.StartAddress).ToList();
            if (items?.Any() != true)
            {
                return;
            }

            var nowTime = DateTime.Now;
            foreach (var item in items)
            {
                var point = item.Value;
                var key = $"{(int)point.FunctionCode}_{point.StartAddress}";
                AlertLogs.TryGetValue(key, out PointAlertLog log);
                ushort? lastValue = log?.ValueLogs?.Any() == true
                    ? log.ValueLogs.FirstOrDefault().Value : null;

                var currentValue = memory.GetPoint(point.StartAddress);
                if (NumbericHelper.HitAlertThreshold(
                    currentValue, lastValue,
                    point.Condition, point.Value, point.LeftValue, point.RightValue))
                {

                    if (log is null)
                    {
                        log = new PointAlertLog
                        {
                            ModbusKey = this.ModbusKey,
                            FunctionCode = (int)point.FunctionCode,
                            Address = (ushort)point.StartAddress
                        };

                        AlertLogs[key] = log;
                    }

                    if (log.ValueLogs.IsEmpty
                        || (log.ValueLogs.TryPeek(out KeyValuePair<DateTime, ushort> logItem)
                            && NumbericHelper.HitAlertTime(nowTime - logItem.Key, point.AlertTimes)))
                    {
                        log.ValueLogs.Push(new KeyValuePair<DateTime, ushort>(nowTime, currentValue));

                        if (!lastValue.HasValue && point.Condition == AlertCondition.Changed)
                        {
                            continue;
                        }

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UiGlobalContext.EnqueueRootMessage(
                                $"[{nowTime:yyyy-MM-dd HH:mm:ss}] !!!警告 EMS:{ModbusKey} " +
                                $"功能码:{(int)point.FunctionCode} 地址:{point.StartAddress}" +
                                $" 当前值:{currentValue} 警告!!!");
                        });
                    }

                }
            }
        }

        private void TrySendMesage(
            string uniqueKey, ModbusFunctionCode functionCode, int startAddr,
            ReadOnlyMemory<ushort> memory, bool forceSend = false, string token = null)
        {
            var bufferObj = PatitionBuffer[uniqueKey];
            bufferObj.TryEnqueue(memory, false);

            var nowtime = DateTime.Now;
            var renderInterval = ConfigContext.GenericConfig.LivingRenderInterval;
            if (!_sendLogDict.TryGetValue(uniqueKey, out var lastTime))
            {
                lastTime = nowtime.AddMilliseconds(renderInterval * -1);
            }

            if (forceSend || (nowtime - lastTime).TotalMilliseconds >= renderInterval)
            {
                var succeed = bufferObj.TryDequeue(out var buffer);

                ModbusGroupContext.SendMessage(functionCode, startAddr, buffer, token);

                _sendLogDict[uniqueKey] = nowtime;
            }

            bufferObj.ClearTail();
            //Debug.WriteLine($"ModbusDataStorage {uniqueKey} count: {bufferObj.Count}");
        }

        private void WriteLog(string uniqueKey, ReadOnlyMemory<ushort> buffer)
        {
            if (_loggerDict.TryGetValue(uniqueKey, out ILogger logger))
            {
                logger.Information(buffer.ToLogString());
            }
        }

        public void ClearBuffers(IEnumerable<string> keys = null)
        {
            var clearAll = keys is null;

            foreach (var item in PatitionBuffer)
            {
                if (clearAll || keys.Contains(item.Key))
                {
                    item.Value?.Clear();
                }
            }
        }

        public ReadOnlyMemory<ushort> Dequeue(string key)
        {
            if (PatitionBuffer.TryGetValue(key, out var values)
                && values.TryDequeue(out var result))
            {
                return result;
            }

            return Empty;
        }
    }
}
