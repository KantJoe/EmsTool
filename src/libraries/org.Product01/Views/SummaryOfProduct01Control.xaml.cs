using org.Product01.ViewModels;
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

namespace org.Product01.Views
{
    /// <summary>
    /// SummaryOfProduct01Control.xaml 的交互逻辑
    /// </summary>
    public partial class SummaryOfProduct01Control : UserControl
    {
        private Product01SystemSummaryViewModel _vm;
        public SummaryOfProduct01Control()
        {
            this.Resources.MergedDictionaries.Clear();
            InitializeComponent();

            _vm = new Product01SystemSummaryViewModel();
            DataContext = _vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.Load();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _vm.UnLoad();
        }
    }
}
