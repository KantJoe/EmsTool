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
    /// EnergyMovingOfProduct01Control.xaml 的交互逻辑
    /// </summary>
    public partial class EnergyMovingOfProduct01Control : UserControl
    {
        private EmProduct01ViewModel _vm;
        public EnergyMovingOfProduct01Control()
        {
            this.Resources.MergedDictionaries.Clear();
            InitializeComponent();

            _vm = new EmProduct01ViewModel();
            DataContext = _vm;
        }
    }
}
