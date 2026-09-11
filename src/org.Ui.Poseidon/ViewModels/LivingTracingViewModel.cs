using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Communication;
using org.Models;
using org.Ui.ViewModels;
using MaterialDesignThemes.Wpf;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;
using org.Utils;
using System.Diagnostics;
using org.Utils.Global;
using Serilog;
using Serilog.Core;
using System.IO;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class LivingTracingViewModel : ObservableObject
    {
        private DispatcherTimer _dTimer;

        private Dictionary<string, DataLogger> _dataLogger;

        private Dictionary<string, SemaphoreSlim> _runningLock;

        private DateTime _stopTime;

        private ILogger _logger;


        [ObservableProperty]
        private ObservableCollection<LivingTracedPointViewModel> _points;

        [ObservableProperty]
        private LivingTracedPointViewModel _selectedPoint;

        [ObservableProperty]
        private int _interval;

        [ObservableProperty]
        private bool _isRunning;

        [ObservableProperty]
        private int _timeSpanMinutes;

        [ObservableProperty]
        private string _logFilePath;

        [ObservableProperty]
        private int _minimumInterval;

        public WpfPlot Content { get; set; }

        public LivingTracingViewModel()
        {
            StartTracingCommand = new AsyncRelayCommand(StartTracing);
            StopTracingCommand = new AsyncRelayCommand(StopTracing);
            ViewSwitchCommand = new AsyncRelayCommand<object>(ViewSwitch);

            Content = new();
            Content.Plot.DataBackground.Color = new ScottPlot.Color(System.Drawing.Color.DimGray);
            Content.Background = new SolidColorBrush(System.Windows.Media.Colors.DimGray);
            TimeSpanMinutes = 1;
            Interval = 50;
        }

        private async void Ticktok(object sender, EventArgs e)
        {
            _dTimer.IsEnabled = false;
            var log = new StringBuilder();
            try
            {
                var oaDate = DateTime.Now.ToOADate();
                foreach (var dataLogger in _dataLogger)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    var keys = dataLogger.Key.Split("_");
                    var key = keys[0];
                    var functionCode = (ModbusFunctionCode)int.Parse(keys[1]);
                    var address = ushort.Parse(keys[2]);
                    if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(key, out var obj)
                        || obj is not IModbusObject modbus)
                    {
                        log.Append(",");
                        continue;
                    }

                    ReadOnlyMemory<ushort> val = functionCode == ModbusFunctionCode.ReadHoldingRegisters ?
                        await modbus.ReadHoldingRegisters(modbus.Options.SlaveId, address, 1)
                        : await modbus.ReadInputRegisters(modbus.Options.SlaveId, address, 1);

                    var point = val.GetPoint();
                    log.Append(point + ",");
                    dataLogger.Value.Add(new Coordinates(oaDate, point));

                    sw.Stop();
                    await Task.Delay(Interval - Math.Min(Interval, (int)sw.Elapsed.TotalMilliseconds));
                }

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "LivingTracingViewModel.Ticktok");
            }
            finally
            {
                _logger.Information(log.ToString().TrimEnd(','));
                Content.Refresh();
                _dTimer.IsEnabled = true;

                if ((DateTime.Now - _stopTime).TotalSeconds > 0)
                {
                    await StopTracing();
                }
            }
        }

        private ScottPlot.DataGenerators.RandomWalker _walker;
        public IAsyncRelayCommand StartTracingCommand { get; set; }
        private async Task StartTracing()
        {
            if (!CommunicateAdapterPool.Online)
            {
                return;
            }

            Content.Reset();
            _dataLogger = new Dictionary<string, DataLogger>();
            _runningLock = new Dictionary<string, SemaphoreSlim>();

            LogFilePath = Path.Combine(
                FilePathConst.LivingTracing_Directory,
                $"{DateTime.Now:yyyyMMdd-HHmmss}.csv");
            _logger = LogFactory.CreateExtensLogger(
                LogFilePath, "[{Timestamp:HH:mm:ss.fff}],{Message}{NewLine}");

            var log = new StringBuilder();
            foreach (var item in Points.Where(w => w.IsSelected))
            {
                var key = item.Point.Key;
                var dataLogger = Content.Plot.Add.DataLogger();
                dataLogger.Color = item.Color;
                var dataKey = $"{key}_{(int)item.Point.FunctionCode}_{item.Point.Address}";
                _dataLogger.Add(dataKey, dataLogger);
                log.Append(dataKey + ",");

                if (!_runningLock.ContainsKey(key))
                {
                    var @lock = ConsistencyContext.Instance.GetInstance(lockKey: item.Point.Key);
                    await @lock.WaitAsync(-1);
                    _runningLock[key] = @lock;
                }
            }

            _logger.Information(log.ToString().TrimEnd(','));

            if (_dataLogger?.Any() != true)
            {
                return;
            }

            _walker = new(1, multiplier: 1000);
            Content.Plot.Axes.DateTimeTicksBottom();
            Content.Plot.RenderManager.RenderStarting += (s, e) =>
            {
                Tick[] ticks = Content.Plot.Axes.Bottom.TickGenerator.Ticks;
                for (int i = 0; i < ticks.Length; i++)
                {
                    DateTime dt = DateTime.FromOADate(ticks[i].Position);

                    // 自定义标签格式，这里显示分钟和秒
                    string label = $"{dt:HH:mm:ss}";

                    // 更新刻度标签
                    ticks[i] = new Tick(ticks[i].Position, label);
                }
            };
            Content.Plot.DataBackground.Color = new ScottPlot.Color(System.Drawing.Color.DarkGray);
            Content.Background = new SolidColorBrush(System.Windows.Media.Colors.DarkGray);

            _dTimer = new DispatcherTimer() { IsEnabled = false };
            _dTimer.Interval = TimeSpan.FromMilliseconds(Interval * _dataLogger.Count);
            _dTimer.Tick += Ticktok;

            _stopTime = DateTime.Now.AddMinutes(TimeSpanMinutes);

            IsRunning = true;
            UiGlobalContext.Instance.IsLivingTraced = true;
            _dTimer.Start();
        }

        public IAsyncRelayCommand StopTracingCommand { get; set; }
        private async Task StopTracing()
        {
            _dTimer?.Stop();
            foreach (var item in _runningLock.Values)
            {
                item?.Release();
            }

            UiGlobalContext.Instance.IsLivingTraced = false;
            IsRunning = false;
        }

        public IAsyncRelayCommand<object> ViewSwitchCommand { get; set; }
        private async Task ViewSwitch(object obj)
        {

        }

        public void Initialize()
        {
            var platter = new ScottPlot.Palettes.Category10();
            var index = 0;
            Points = [.. LivingTracingContext.Instance.Points
                .Select(s => new LivingTracedPointViewModel(s, platter.GetColor(index++)))
                .OrderBy(o => o.Point.Key)
                .ThenBy(t => t.Point.FunctionCode)
                .ThenBy(t => t.Point.Address)
                .ToList()];

            MinimumInterval = EmsSolutionContext.Current.Settings.LivingTracingMinimumInterval;
        }

        public void Unload()
        {
            if (_dTimer?.IsEnabled == true)
            {
                _dTimer.Stop();
                _dTimer.IsEnabled = false;
                _dTimer.Tick -= Ticktok;
            }

            _dTimer = null;
            Content?.Reset();
        }
    }
}
