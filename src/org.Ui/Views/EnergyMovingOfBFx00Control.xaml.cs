using org.Ui.ViewModels;
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

namespace org.Ui.Views
{
    /// <summary>
    /// EnergyMovingOfBFx00Control.xaml 的交互逻辑
    /// </summary>
    public partial class EnergyMovingOfBFx00Control : UserControl
    {
        private EmBFx00ViewModel _vm;
        public EnergyMovingOfBFx00Control(ProductModelViewModel model)
        {
            InitializeComponent();

            _vm = new EmBFx00ViewModel(model);
            DataContext = _vm;
        }
    }
}
