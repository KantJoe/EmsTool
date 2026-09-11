using org.Privilege.ViewModels;
using org.Ui;
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

namespace org.Privilege.Views
{
    /// <summary>
    /// PriviledgeManageControl.xaml 的交互逻辑
    /// </summary>
    public partial class PriviledgeManageControl : UserControl
    {
        private PriviledgeManageViewModel _vm;
        public PriviledgeManageControl()
        {
            InitializeComponent();
            _vm = new PriviledgeManageViewModel();
            DataContext = _vm;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_vm.IsInitialed)
            {
                return;
            }

            await DialogHost.Show(new WaitDialog(), UiGlobalContext.RootDialog, new DialogOpenedEventHandler(async (s, e) =>
            {
                await _vm.InitializeCommand.ExecuteAsync(null);
            }));
        }
    }
}
