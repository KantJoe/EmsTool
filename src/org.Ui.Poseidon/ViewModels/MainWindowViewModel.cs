using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Communication;
using org.Privilege.Attributes;
using org.Privilege.Views;
using org.Ui.MultiLanguage;
using org.Ui.Poseidon.Views;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MenuItem> _menuItems;

        private MenuItem _selectedMenu;
        public MenuItem SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                if (_selectedMenu != value)
                {
                    SetProperty(ref _selectedMenu, value);

                    if (value?.ContentType == typeof(HomeControl))
                    {
                        GC.Collect(2, GCCollectionMode.Forced);
                    }
                }
            }
        }

        [ObservableProperty]
        private bool _menuOpened;

        public object MainWindowDialog { get; set; }


        public MainWindowViewModel()
        {
            SwitchLanguageCommand = new AsyncRelayCommand(SwitchLanguage);
            MenuSelectedCommand = new AsyncRelayCommand<object>(MenuSelected);
            ConnectAllCommand = new AsyncRelayCommand<object>(ConnectAll);

            //var langVMs = ConfigContext.GenericConfig.MultipleLang.Supports
            //    .Select(s => new MultiLangResourceViewModel(s));
            //LangDictionaries = [.. langVMs];
            //SelectedLang = LangDictionaries.FirstOrDefault(f => f.Code == ConfigContext.GenericConfig.MultipleLang.DefaultCode);

            ReInitialize();
        }

        public void ReInitialize()
        {
            GenerateMenus();
        }

        private void GenerateMenus()
        {
            List<MenuItem> list = [
                new MenuItem(){
                    OrderNumber=0,
                    Code="菜单_主界面",
                    Reserved=true,
                    ContentType=typeof(HomeControl),
                    SelectedIcon=PackIconKind.Home,
                    UnselectedIcon=PackIconKind.HomeOutline
                },
                new MenuItem(){
                    Code="菜单_看板",
                    ContentType=typeof(DashboardControl),
                    SelectedIcon=PackIconKind.ChartBox,
                    UnselectedIcon=PackIconKind.ChartBoxOutline
                },
                //new MenuItem(){
                //    Code="菜单_点位表",
                //    ContentType=typeof(PointMapManagerControl),
                //    SelectedIcon=PackIconKind.ViewGrid,
                //    UnselectedIcon=PackIconKind.ViewGridOutline,
                //},
                new MenuItem(){
                    Code="菜单_设备状态",
                    ContentType=typeof(SystemSummaryControl),
                    SelectedIcon = PackIconKind.HomeBattery,
                    UnselectedIcon = PackIconKind.HomeBatteryOutline
                },
                new MenuItem(){
                    Code="菜单_系统设置",
                    ContentType=typeof(SystemSettingControl),
                    SelectedIcon = PackIconKind.WrenchCog,
                    UnselectedIcon = PackIconKind.WrenchCogOutline
                },
                new MenuItem(){
                    Code="菜单_点位表",
                    ContentType=typeof(PointMapBusControl),
                    SelectedIcon=PackIconKind.ViewGrid,
                    UnselectedIcon=PackIconKind.ViewGridOutline,
                },
                new MenuItem(){
                    Code="菜单_告警监控",
                    ContentType=typeof(AlertControl),
                    SelectedIcon=PackIconKind.BellAlert,
                    UnselectedIcon=PackIconKind.BellAlertOutline,
                },
                new MenuItem(){
                    Code="菜单_EMS故障历史",
                    ContentType=typeof(FaultHistoryControl),
                    SelectedIcon=PackIconKind.ClipboardTextClock,
                    UnselectedIcon=PackIconKind.ClipboardTextClockOutline,
                },
                new MenuItem(){
                    Code="菜单_实时监控",
                    Reserved=true,
                    ContentType=typeof(LivingTracingControl),
                    SelectedIcon=PackIconKind.ChartAreasplineVariant,
                    UnselectedIcon=PackIconKind.ChartLine,
                },
                new MenuItem(){
                    Code="菜单_固件升级",
                    ContentType=typeof(UpgradeFirmwareControl),
                    SelectedIcon=PackIconKind.RefreshCircle,
                    UnselectedIcon=PackIconKind.Refresh
                },
                new MenuItem(){
                    Code="菜单_测试自动化"
                },
                new MenuItem(){
                    Code="菜单_通信模拟",
                    Reserved=true,
                    ContentType=typeof(MqttSimulateControl),
                    SelectedIcon=PackIconKind.Wifi,
                    UnselectedIcon=PackIconKind.WifiStrengthOutline,
                },
                new MenuItem(){
                    Code="菜单_阈值和趋势"
                },
                new MenuItem(){
                    Code="菜单_脚本引擎"
                },
                new MenuItem(){
                    Code="菜单_日志"
                },
                new MenuItem(){
                    ContentType=typeof(SettingsControl),
                    Code="菜单_软件设置",
                    SelectedIcon=PackIconKind.Cog,
                    UnselectedIcon=PackIconKind.CogOutline,
                },
                new MenuItem(){
                    ContentType=typeof(PriviledgeManageControl),
                    Code="菜单_权限控制",
                    SelectedIcon=PackIconKind.ShieldAccount,
                    UnselectedIcon=PackIconKind.ShieldAccountOutline,
                },
                new MenuItem(){
                    ContentType=typeof(DeviceTopologyControl),
                    Code="菜单_设备拓扑",
                    SelectedIcon=PackIconKind.PlusNetwork,
                    UnselectedIcon=PackIconKind.PlusNetworkOutline,
                }
                ];
            int i = 1;
            foreach (var item in list)
            {
                item.OrderNumber = i++;
            }

            MenuItems = [.. list];


        }

        public IAsyncRelayCommand<object> ConnectAllCommand { get; set; }
        private async Task ConnectAll(object parms)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(200));
                if (CommunicateAdapterPool.ModbusMasterPool.Values
                    .Where(w => w is IModbusObject)
                    .Any(a => (a?.Online ?? false)))
                {
                    await CommunicateAdapterPool.ClearModbusMastersAsync();
                    await CommunicateAdapterPool.DisconnectAllModbusMastersAsync();
                    return;
                }

                await CommunicateAdapterPool.InitializeModbusMastersAsync();

                await CommunicateAdapterPool.ReconnectModbusMastersAsync();

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "MainWindowViewModel.ConnectAll");
                MessageBox.Show("连接失败，请检查设备上电及连接参数后重试");
            }
        }

        public IAsyncRelayCommand<object> MenuSelectedCommand { get; set; }
        private async Task MenuSelected(object obj)
        {
            MenuOpened = false;
        }

        public IAsyncRelayCommand SwitchLanguageCommand { get; set; }
        private async Task SwitchLanguage()
        {
            MultiLang.SwitchLanguage();
        }

        [RelayCommand]
        public void Priviledge()
        {
            PriviledgeCore();
        }

        [Priviledge(PriviledgeKey = nameof(ApplicationPriviledge.Main_Test))]
        private void PriviledgeCore()
        {
            UiGlobalContext.EnqueueRootMessage("正常执行,拥有此权限");
        }
    }
}
