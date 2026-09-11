using org.Ui.Poseidon.ViewModels;
using MaterialDesignThemes.Wpf;
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

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// PointMapSingleControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointMapSingleControl : UserControl
    {
        private PointMapViewModel _vm;
        public PointMapSingleControl(PointMapViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = vm;

        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(() =>
            //{
            //    _vm.SingleViewLoading();
            //    DialogHost.Close(_vm.Identifier);
            //}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
