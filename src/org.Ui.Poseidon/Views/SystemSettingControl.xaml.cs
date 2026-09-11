using org.Ui.Poseidon.ViewModels;
using org.Ui.ViewModels;
using org.Utils.Global;
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
    /// SystemSettingControl.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSettingControl : UserControl
    {
        private SystemSettingViewModelBase _vm;
        public SystemSettingControl()
        {
            InitializeComponent();
            try
            {
                var solution = EmsSolutionContext.Current;
                var protocol = solution?.DeviceTopologies?.FirstOrDefault()?.EmsProtocol;
                if (ModuleContext.Instance.GetModule(protocol) is IMenuViewModel menuVM)
                {
                    _vm = menuVM.SystemSettingViewModelInitialize();
                }

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingControl.Loaded");
            }

            if (_vm is null)
            {
                _vm = new SystemSettingViewModel();
            }

            DataContext = _vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.Load();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _vm.Unload();
        }
    }
}
