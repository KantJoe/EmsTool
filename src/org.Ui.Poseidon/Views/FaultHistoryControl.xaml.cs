using org.Ui.Poseidon.ViewModels;
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
    /// FaultHistoryControl.xaml 的交互逻辑
    /// </summary>
    public partial class FaultHistoryControl : UserControl
    {
        private FaultHistoryViewModel _vm;
        public FaultHistoryControl()
        {
            InitializeComponent();
            _vm = new FaultHistoryViewModel();
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
