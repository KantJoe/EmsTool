using org.Communication.Extensions;
using org.Models;
using org.Ui;
using org.Ui.MultiLanguage;
using org.Utils;
using org.Utils.Global;
using Flurl.Util;
using OpenTK.Graphics.ES11;
using ScottPlot.Colormaps;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TouchSocket.Core;
using TouchSocket.Modbus;
using TouchSocket.SerialPorts;
using TouchSocket.Sockets;

namespace org.Communication
{
    public class ModbusIntervalPollingPlugin : PluginBase, ISerialConnectedPlugin, ISerialClosedPlugin
    {
        private long POLLING_NO = 0;
        private Thread _pollingThread;
        private string _key;
        private int POLLING_SLEEPMS = EmsSolutionContext.Current?.Settings?.ModbusPollingInterval ?? 100;
        private bool OPTIMIZE_SWITCH = EmsSolutionContext.Current?.Settings?.ModbusPollingOptimizeSwitch ?? false;
        private int OPTIMIZE_VALUE = EmsSolutionContext.Current?.Settings?.ModbusPollingOptimizeValue ?? 2;
        /// <summary>
        /// key: _key + "_"+functionCode+"_"+startAddress
        /// value: 
        /// </summary>
        private Dictionary<string, PointMapRangMapping> _registerMappings;
        private CancellationTokenSource _tokenSource;
        private byte _slaveId;

        private void PollingCore(object clientObj)
        {
            int spanMs = 0;
            int disconnectCount = 0;
            var client = clientObj as IModbusMaster;

            var storage = CommunicateAdapterPool.StoragePool[_key];

            var currentSolution = EmsSolutionContext.Current;
            POLLING_SLEEPMS = currentSolution?.Settings?.ModbusPollingInterval ?? 100;
            OPTIMIZE_SWITCH = currentSolution?.Settings?.ModbusPollingOptimizeSwitch ?? false;
            OPTIMIZE_VALUE = currentSolution?.Settings?.ModbusPollingOptimizeValue ?? 2;
            var logDict = CreateLogger();

            // 从空闲转运行中
            while (disconnectCount < 2 && GetLock(out var @lock))
            {
                try
                {
                    if (ConnectState(client))
                    {
                        spanMs = ReadDataAsync(client, storage).ConfigureFalseAwait()
                            .GetAwaiter().GetResult();

                        disconnectCount = 0;
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] mipp.Polling {POLLING_NO++}: span " + spanMs);
                    }
                    else
                    {
                        spanMs = -1000;
                        disconnectCount++;
                    }
                }
                catch (Exception ex)
                {
                    LogFactory.Error(ex, $"ModbusIntervalPollingPlugin.PollingCore {_key}：{Thread.CurrentThread.ManagedThreadId} Error");
                }
                finally
                {
                    // 从运行中转空闲
                    ReleaseLock(@lock);
                    Thread.Sleep(GetSleepMs(POLLING_SLEEPMS - spanMs - 5, POLLING_SLEEPMS));
                    spanMs = 0;
                }
            }

            _pollingThread = null;
        }
        private async Task<int> ReadDataAsync(
            IModbusMaster client, ModbusDataStorage storage, ushort lengthPerGroup = 125)
        {
            var sw = Stopwatch.StartNew();

            var bucks = new List<KeyValuePair<string, ReadOnlyMemory<ushort>>>();
            foreach (var mappingPair in _registerMappings)
            {
                if (mappingPair.Value.MapLimitation > 0)
                {
                    continue;
                }

                if (client is not IOnlineClient cc
                    || !cc.Online)
                {
                    return (int)sw.Elapsed.TotalMilliseconds;
                }

                var map = mappingPair.Value;

                // mapping中的slaveid有可能因为设置与连接的参数修改导致不同步，
                // 通过使用连接时的实际slaveid更加可靠
                var buff =
                    map.FunctionCode == ModbusFunctionCode.ReadHoldingRegisters
                    ? await client.ReadHoldingRegistersDataAsync(
                        _slaveId, (ushort)map.StartAddress, (ushort)map.PointCount, 200)
                    : await client.ReadInputRegistersDataAsync(
                        _slaveId, (ushort)map.StartAddress, (ushort)map.PointCount, 200);

                bucks.Add(new(mappingPair.Key, buff));

                if (!OPTIMIZE_SWITCH)
                {
                    await Task.Delay(OPTIMIZE_VALUE);
                }
                //Debug.WriteLine($"mipp: {map.StartAddress} {mappingPair.Value.FunctionCode} result:{response.IsSuccess},{response.ErrorCode}");
            }

            _ = Task.Factory.StartNew(() =>
            {
                storage.ReceivingBuck([.. bucks], token: _key);
            }, CancellationToken.None);

            sw.Stop();
            return (int)sw.Elapsed.TotalMilliseconds;
        }

        static ReadOnlyMemory<ushort> BUFFER_EMPTY = new ushort[125];
        private int EmptyReadData(ModbusDataStorage storage, ushort lengthPerGroup = 125, bool forceSend = false)
        {
            var bucks = _registerMappings.Select(
                s => new KeyValuePair<string, ReadOnlyMemory<ushort>>(s.Key, BUFFER_EMPTY));

            storage.ReceivingBuck([.. bucks], forceSend, token: _key);
            return -1000;
        }

        private Dictionary<string, ILogger> CreateLogger()
        {
            var dict = new Dictionary<string, ILogger>();
            _registerMappings = new Dictionary<string, PointMapRangMapping>();
            var currentSolution = EmsSolutionContext.Current;

            var mappings = currentSolution?.Settings?.PointMapMappings;
            if (mappings?.Any() != true)
            {
                return dict;
            }

            foreach (var mapping in mappings)
            {
                var key = _key + "_" + (int)mapping.FunctionCode + "_" + mapping.StartAddress;
                dict[key] = LogFactory.CreateLogger(
                    key, FilePathConst.CsvExt,
                    "[{Timestamp:HH:mm:ss.fff}],{Message}{NewLine}");
                _registerMappings[key] = mapping;
            }

            return dict;
        }

        private bool GetLock(out SemaphoreSlim @lock)
        {
            @lock = ConsistencyContext.Instance.GetInstance(lockKey: _key);
            return !(_tokenSource?.IsCancellationRequested ?? false) && @lock.Wait(-1, _tokenSource.Token);
        }

        private bool ReleaseLock(SemaphoreSlim @lock)
        {
            if (@lock.CurrentCount > 0)
            {
                return true;
            }

            return @lock.Release() == 0;
        }

        private int GetSleepMs(int calcMs, int defMs)
        {
            return Math.Max(1, Math.Min(calcMs, defMs));
        }

        private bool TryReleaseAll()
        {
            LogFactory.Info(
                $"ModbusIntervalPollingPlugin.TryRelease: {_key}");

            return ConsistencyContext.Instance
                .ReleaseSemaphore(_key, releasingAction: () =>
                {
                    _tokenSource?.Cancel();
                });
        }

        private void ReInitializeThread(ISerialPortSession client)
        {
            var storage = CommunicateAdapterPool.StoragePool[_key];
            storage.ClearBuffers();

            TryReleaseAll();

            _pollingThread = new Thread(new ParameterizedThreadStart(Polling))
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            _pollingThread.Start(client);
        }

        private void Polling(object obj)
        {
            try
            {
                _tokenSource?.Cancel();
                _tokenSource = new CancellationTokenSource(-1);

                PollingCore(obj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"mipp polling,{ex.Message}");
                LogFactory.Error(ex, "ModbusIntervalPollingPlugin.Polling");
            }

            UiGlobalContext.Instance.Online = CommunicateAdapterPool.Online;

            if( CommunicateAdapterPool.StoragePool.TryGetValue(_key,out var storage))
            {
                Task.Run(() =>
                {
                    Task.Delay(1000).Wait();
                    storage?.ClearBuffers();
                    EmptyReadData(storage, forceSend: true);
                });
                Application.Current.Dispatcher.Invoke(() => UiGlobalContext.EnqueueRootMessage(_key + " " + MultiLang.GetString("连接已关闭")));
            }

            Debug.WriteLine($"{_key} stop polling {POLLING_NO}");
        }

        private static bool ConnectState(IModbusMaster client)
        {
            if (client is ModbusRtuMaster rtu)
            {
                return rtu.Online;
            }

            if (client is ModbusTcpMaster tcp)
            {
                return tcp.Online;
            }

            return false;
        }

        public async Task OnSerialConnected(ISerialPortSession client, ConnectedEventArgs e)
        {
            UiGlobalContext.Instance.Online = CommunicateAdapterPool.Online;
            foreach (var item in CommunicateAdapterPool.ModbusMasterPool)
            {
                if (item.Value is ModbusRtuObject rtuObj
                    && rtuObj.Instance == client)
                {
                    _key = item.Key;
                }
                else if (item.Value is ModbusTcpObject tcpObj
                    && tcpObj.Instance == client)
                {
                    _key = item.Key;
                }

                _slaveId = item.Value.Options?.SlaveId ?? 1;
            }

            LogFactory.Info(
                $"ModbusIntervalPollingPlugin.OnSerialConnected: {_key}");

            ReInitializeThread(client);
        }

        protected override void Loaded(IPluginManager pluginManager)
        {
            LogFactory.Info("ModbusIntervalPollingPlugin.Loaded");
        }

        protected override void Unloaded(IPluginManager pluginManager)
        {
            LogFactory.Info(
                $"ModbusIntervalPollingPlugin.UnLoaded: {_key}");
            TryReleaseAll();
        }

        public async Task OnSerialClosed(ISerialPortSession client, ClosedEventArgs e)
        {
            TryReleaseAll();
            Debug.WriteLine("mipp.release finished");

        }
    }
}
