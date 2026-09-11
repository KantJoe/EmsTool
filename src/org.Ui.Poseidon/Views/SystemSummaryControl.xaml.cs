using org.Ui.Poseidon.ViewModels;
using org.Ui.ViewModels;
using org.Ui.Views;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// SystemSummaryControl.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSummaryControl : UserControl
    {
        public SystemSummaryControl()
        {
            InitializeComponent();

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await UiGlobalContext.ShowRootDialog(new WaitDialog(), openHandler: new DialogOpenedEventHandler(async (s, e) =>
            {
                await Task.Delay(500);

                var solution = EmsSolutionContext.Current;
                var device = solution?.DeviceTopologies?.FirstOrDefault();
                if (device is null)
                {
                    return;
                }


                if (ModuleContext.Instance.GetModule(device.EmsProtocol) is not IMenuControl moduleControl)
                {
                    return;
                }

                ContentControl.Content = moduleControl.CreateSystemSummaryControl(null);

                await Task.Delay(500);
                e.Session.Close(0);
            }));

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
