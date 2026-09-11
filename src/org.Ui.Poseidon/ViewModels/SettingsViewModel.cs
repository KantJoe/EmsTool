using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Models;
using org.Models.Athena;
using org.Ui.ViewModels;
using org.Utils;
using org.Utils.Global;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        #region 交互
        [ObservableProperty]
        private int _livingRenderInterval;

        [ObservableProperty]
        private bool _modbusPollingOptimizeSwitch;

        [ObservableProperty]
        private int _modbusPollingOptimizeValue;

        [ObservableProperty]
        private int _livingTracingMinimumInterval;
        #endregion


        #region 工作区
        [ObservableProperty]
        private int _modbusPollingInterval;
        [ObservableProperty]
        private int _lengthOfPointMapGroup;

        [ObservableProperty]
        private uint _manualFunctionCode;

        [ObservableProperty]
        private ushort _manualStartAddress;

        [ObservableProperty]
        private ushort _manualEndAddress;

        [ObservableProperty]
        private ObservableCollection<PointMapRangMappingViewModel> _pointMapRang;

        public IAsyncRelayCommand PointMapRangeGenerateCommand { get; set; }
        private async Task PointMapRangeGenerate()
        {
            var device = EmsSolutionContext.Current?.DeviceTopologies
                ?.FirstOrDefault();
            var module = ModuleContext.Instance.GetModule(device.EmsProtocol);
            if (module is null)
            {
                return;
            }

            var setting = module.ResetEmsSetting();
            PointMapRang = [.. setting.PointMapMappings.Select(s => new PointMapRangMappingViewModel(s))];
        }

        public IAsyncRelayCommand<PointMapRangMappingViewModel> DeletePointMapGroupCommand { get; set; }
        private async Task DeletePointMapGroup(PointMapRangMappingViewModel mapVM)
        {
            mapVM.MapLimitation = 1;
        }

        public IAsyncRelayCommand AppendPointRangeCommand { get; set; }
        private async Task AppendPointRange()
        {
            var device = EmsSolutionContext.Current?.DeviceTopologies
                ?.FirstOrDefault();

            var slaveId = device.ConnectionOptions.SlaveId;
            var list = new List<PointMapRangMappingViewModel>();
            var pages = (ManualEndAddress - ManualStartAddress + 1) / 125;
            var items = Enumerable.Range(0, pages);
            foreach (var item in items)
            {
                list.Add(new PointMapRangMappingViewModel(new PointMapRangMapping
                {
                    FunctionCode = (ModbusFunctionCode)ManualFunctionCode,
                    StartAddress = ManualStartAddress + item * 125,

                }));
            }

            PointMapRang = [.. PointMapRang.Concat(list)
                .OrderBy(o => o.FunctionCode)
                .ThenBy(t => t.StartAddress)
                .ToList()];
        }
        #endregion

        #region others
        [ObservableProperty]
        private bool _others_CheckUpdate;

        [ObservableProperty]
        private string _newerVersion;

        [ObservableProperty]
        private Visibility _upgradeVisibility;

        [ObservableProperty]
        private Visibility _upgradingVisibility;

        [ObservableProperty]
        private double _downloadProgress;


        public IAsyncRelayCommand UpgradeCommand { get; set; }
        private async Task Upgrade()
        {
            DownloadProgress = 0;
            UpgradingVisibility = Visibility.Visible;
            var url = string.Format(ConfigContext.GenericConfig.EmsServer +
                ":" + ConfigContext.GenericConfig.EmsServerPort +
                ConfigContext.GenericConfig.DownloadUrl,
                $"PoseidonEms-{ConfigContext.NewestAppVersion.MajorVersion}." +
                $"{ConfigContext.NewestAppVersion.MinorVersion}." +
                $"{ConfigContext.NewestAppVersion.BuildVersion}",
                ConfigContext.NewestAppVersion.Asset);

            if (!Directory.Exists(FilePathConst.NewAppDirectory))
            {
                Directory.CreateDirectory(FilePathConst.NewAppDirectory);
            }

            var list = new List<Task>();
            var timeout = 100;
            var fileName = string.Empty;
            list.Add(Task.Run(async () =>
            {
                for (var index = 0; index < timeout; index++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    DownloadProgress += 1;
                }
            }));
            list.Add(Task.Run(async () =>
            {
                fileName = await HttpHelper.DownloadNewerAppAsync(url, ConfigContext.NewestAppVersion.Asset, timeout);
            }));

            try
            {
                await Task.WhenAny(list);
                DownloadComplete(fileName);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "HttpHelper.Upgrade");
            }
        }

        private void DownloadComplete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            if (!File.Exists(fileName))
            {
                return;
            }

            DownloadProgress = 100;
            var folder = Path.Combine(
                FilePathConst.NewAppDirectory, Path.GetFileNameWithoutExtension(fileName));
            var result = ZipUtil.UnzipFile(fileName, folder);

            var targetFile = result?.FirstOrDefault(
                f => Path.GetFileNameWithoutExtension(fileName) == Path.GetFileNameWithoutExtension(f));
            if (targetFile is null
                || !File.Exists(targetFile))
            {
                Directory.Delete(folder);
                return;
            }

            LogFactory.Info($"下载文件成功,正在打开安装程序并退出当前程序 TargetFile: {targetFile}");
            var process = Process.Start(targetFile);
            Environment.Exit(0);
        }
        #endregion

        public SettingsViewModel()
        {
            SaveSettingCommand = new AsyncRelayCommand(SaveSettingAsync);
            ResetSettingCommand = new AsyncRelayCommand(ResetSetting);
            UpgradeCommand = new AsyncRelayCommand(Upgrade);
            PointMapRangeGenerateCommand = new AsyncRelayCommand(PointMapRangeGenerate);
            DeletePointMapGroupCommand = new AsyncRelayCommand<PointMapRangMappingViewModel>(DeletePointMapGroup);
            AppendPointRangeCommand = new AsyncRelayCommand(AppendPointRange);

            UpgradeVisibility = Visibility.Collapsed;
            UpgradingVisibility = Visibility.Collapsed;

            ReloadConfig();
        }

        public void ReloadConfig()
        {
            var solution = EmsSolutionContext.Current;
            if (solution is not null)
            {
                ModbusPollingInterval = solution.Settings?.ModbusPollingInterval ?? 1000;
                LengthOfPointMapGroup = solution.Settings?.LengthOfPointMapGroup ?? 125;
                ModbusPollingOptimizeSwitch = solution.Settings.ModbusPollingOptimizeSwitch;
                ModbusPollingOptimizeValue = solution.Settings.ModbusPollingOptimizeValue;
                LivingRenderInterval = solution.Settings.LivingTracingMinimumInterval;
                PointMapRang = new ObservableCollection<PointMapRangMappingViewModel>(solution.Settings.PointMapMappings
                    ?.Select(s => new PointMapRangMappingViewModel(s)) ?? new List<PointMapRangMappingViewModel>());
            }


            var config = ConfigContext.GenericConfig;

            LivingRenderInterval = config.LivingRenderInterval;


            Others_CheckUpdate = config.Others_CheckUpdate;
            if (ConfigContext.NewestAppVersion is AppReleaseNote app)
            {
                UpgradeVisibility = Visibility.Visible;
                var revision = string.IsNullOrEmpty(app.Revision) ? "" : ("-" + app.Revision);
                NewerVersion = $"{app.AppName}-{app.MajorVersion}.{app.MinorVersion}.{app.BuildVersion}{revision}";
            }
        }

        private void WriteConfig()
        {
            var emsSetting = EmsSolutionContext.Current?.Settings ?? new Models.EmsSettings();
            emsSetting.ModbusPollingInterval = ModbusPollingInterval;
            emsSetting.LengthOfPointMapGroup = LengthOfPointMapGroup;
            emsSetting.PointMapMappings = PointMapRang.Select(s => s.Mapping).ToList();
            emsSetting.ModbusPollingOptimizeSwitch = ModbusPollingOptimizeSwitch;
            emsSetting.ModbusPollingOptimizeValue = ModbusPollingOptimizeValue;
            emsSetting.LivingTracingMinimumInterval = LivingTracingMinimumInterval;
            EmsSolutionContext.Current.Settings = emsSetting;

            ConfigContext.GenericConfig.Others_CheckUpdate = Others_CheckUpdate;
            ConfigContext.GenericConfig.LivingRenderInterval = LivingRenderInterval;
        }

        public IAsyncRelayCommand SaveSettingCommand { get; set; }
        private async Task SaveSettingAsync()
        {
            WriteConfig();

            await EmsSolutionContext.SaveEmsSolution(EmsSolutionContext.Current);
            await ConfigContext.SaveConfigAsync();
        }

        public IAsyncRelayCommand ResetSettingCommand { get; set; }
        private async Task ResetSetting()
        {
            ReloadConfig();
        }


    }
}
