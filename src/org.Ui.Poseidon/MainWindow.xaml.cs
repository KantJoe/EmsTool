using org.Communication;
using org.Models;
using org.Ui.MultiLanguage;
using org.Ui.Poseidon.ViewModels;
using org.Ui.Poseidon.Views;
using org.Utils;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace org.Ui.Poseidon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int NormalSize_Width = 1280;
        private int NormalSize_Height = 900;
        private MainWindowViewModel _vm;


        public MainWindow()
        {
            InitializeComponent();



            this.Width = NormalSize_Width;
            this.Height = NormalSize_Height;
            this.TbTitle.Text = (ConfigContext.GenericConfig.MainTitle + "_V" + ConfigContext.GenericConfig.Version);
            this.TxtViceTitle.Text = ConfigContext.GenericConfig.SubTitle;
            //PicMain.Source = BitmapFrame.Create(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Resources\\Images\\favicon.ico"), UriKind.Relative));

            this.CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Close,
                (t, e) => this.Close(),
                (s, e) => { e.CanExecute = true; }));
            UiGlobalContext.EnqueueRootMessage = EnqueueMessage;

            _vm = new MainWindowViewModel();
            _vm.MainWindowDialog = BtnLink.CommandParameter;
            DataContext = _vm;
        }

        private void BtnMaximun_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.Width = NormalSize_Width;
                this.Height = NormalSize_Height;

                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void BtnMinimun_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1:
                    this.DragMove();
                    break;
                case 2:
                    BtnMaximun_Click(sender, e);
                    break;
            }
        }

        private async void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            await Relogin1(true);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dpiScalX = (double)PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice.M11;
            var dpiScalY = (double)PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice.M22;
            var currentMaxWidth = SystemParameters.FullPrimaryScreenWidth;
            var currentMaxHeight = SystemParameters.FullPrimaryScreenHeight;
            if (currentMaxHeight < NormalSize_Height || currentMaxWidth < NormalSize_Width)
            {
                var text = $"经推算，当前屏幕物理分辨率为{currentMaxWidth * dpiScalX}*{currentMaxHeight * dpiScalY},\n" +
                    $"缩放比例为Width: {dpiScalX * 100} % 和Height: {dpiScalY * 100} %,\n" +
                    $"无法满足最小宽高{NormalSize_Width}*{NormalSize_Height},\n" +
                    $"请调整后再启动.程序将在5s后自动退出";
                var textBlock = new TextBlock() { Text = text, TextWrapping = TextWrapping.Wrap };
                await UiGlobalContext.ShowRootDialog(
                    textBlock,
                    openHandler: new DialogOpenedEventHandler(async (s, e) =>
                    {
                        await Task.Delay(5000);
                        Application.Current.MainWindow.Close();
                    }));

                return;
            }

            await Relogin1(false);
        }

        private void CheckUpdates()
        {
            if (!ConfigContext.GenericConfig.Others_CheckUpdate)
            {
                return;
            }

            Task.Factory.StartNew(async () =>
                {
                    var url = ConfigContext.GenericConfig.EmsServer + ":" +
                    ConfigContext.GenericConfig.EmsServerPort +
                    ConfigContext.GenericConfig.CheckUpdateUrl +
                    ConfigContext.GenericConfig.MajorVersion + "." +
                    ConfigContext.GenericConfig.MinorVersion + "." +
                    ConfigContext.GenericConfig.BuildVersion;

                    var result = await HttpHelper.CheckUpdateAsync(url);
                    var newestVersion = result?.Data?.FirstOrDefault();
                    if (newestVersion is not null)
                    {
                        ConfigContext.NewestAppVersion = newestVersion;
                        var tip = MultiLang.GetStringFormat(MultiLang.Instance.设置_发现新版本,
                            newestVersion.MajorVersion,
                            newestVersion.MinorVersion,
                            newestVersion.BuildVersion);
                        //note you can use the message queue from any thread, but just for the demo here we 
                        //need to get the message queue from the snackbar, so need to be on the dispatcher
                        MainSnackbar.MessageQueue?.Enqueue(tip);
                    }
                    else if (result is null || result.Code != 0)
                    {
                        MainSnackbar.MessageQueue?.Enqueue(MultiLang.GetString(result?.Msg ?? "获取更新失败，请检查网络! "));
                    }

                }, CancellationToken.None,
                TaskCreationOptions.RunContinuationsAsynchronously,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void EnqueueMessage(string message)
        {
            //note you can use the message queue from any thread, but just for the demo here we 
            //need to get the message queue from the snackbar, so need to be on the dispatcher
            MainSnackbar.MessageQueue?.Enqueue(message);
        }

        private async Task Relogin1(bool reload = true)
        {
            // 此过程存在UI资源延迟释放，导致数据重新初始化与UI资源刷新不一致，
            // 当下较为妥当的方式是使用异步委托的方式执行处理

            await UiGlobalContext.ShowRootDialog(new LoginControl(),
                openHandler: new DialogOpenedEventHandler(async (s, e) =>
                {
                    if (reload)
                    {
                        await CommunicateAdapterPool.DisconnectAllModbusMastersAsync();
                        //await CommunicateAdapterPool.ClearMqttClientsAsync();
                        EmsSolutionContext.SetSolution(default(EmsSolution));
                        LoginAccountContext.Logout();

                        _vm.ReInitialize();
                        _vm.SelectedMenu = _vm.MenuItems.First();

                    }
                }),
                closingHandler: new DialogClosingEventHandler(async (s, e) =>
                {
                    if ((bool)e.Parameter == true)
                    {
                        LivingTracingContext.Instance.Initialize();
                        var solution = EmsSolutionContext.Current;
                        await CommunicateAdapterPool.ClearModbusMastersAsync();
                        await CommunicateAdapterPool.InitializeModbusMastersAsync();

                        AssemblyContext.Instance.LoadAssembley(solution.DeviceTopologies?.FirstOrDefault()?.EmsProtocol);

                        _vm.SelectedMenu = _vm.MenuItems.First();

                        CheckUpdates();
                    }
                }));
        }

        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
            => ModifyTheme(DarkModeToggleButton.IsChecked == true);

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }

    }
}