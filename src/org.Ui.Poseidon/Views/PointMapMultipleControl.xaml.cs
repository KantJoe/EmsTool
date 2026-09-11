using org.Ui.Poseidon.ViewModels;
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
    /// PointMapMultipleControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointMapMultipleControl : UserControl
    {
        private PointMapProxyViewModel _vm;
        public PointMapMultipleControl(PointMapProxyViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //DialogHost.Close(_vm.Identifier);
        }
    }
}
