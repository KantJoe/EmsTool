using org.Ui.Poseidon.ViewModels;
using org.Ui.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// SettingsControl.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private SettingsViewModel _vm;
        public SettingsControl()
        {
            InitializeComponent();

            _vm = new SettingsViewModel();
            DataContext = _vm;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.ReloadConfig();
            DialogHost.Show(new WaitDialog(), UiGlobalContext.RootDialog, new DialogOpenedEventHandler((s, e) =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(500))
                    .ContinueWith((t, _) =>
                    {
                        e.Session.Close(false);
                    }, null, TaskScheduler.FromCurrentSynchronizationContext());
            }));

        }
    }
}
