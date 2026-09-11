using org.Ui.Poseidon.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// RegistersManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointMapManagerControl : UserControl
    {
        private PointMapManagerViewModel _vm;
        public PointMapManagerControl()
        {
            InitializeComponent();

            _vm = new PointMapManagerViewModel();
            DataContext = _vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void TbMain_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.Initialize();
            _vm.SelectedItem = _vm.Items?.FirstOrDefault();
        }

        private void TbMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
