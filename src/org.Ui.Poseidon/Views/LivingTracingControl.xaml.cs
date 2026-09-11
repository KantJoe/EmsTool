using org.Ui.Poseidon.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// LivingTracingControl.xaml 的交互逻辑
    /// </summary>
    public partial class LivingTracingControl : UserControl
    {
        private LivingTracingViewModel _vm;
        public LivingTracingControl()
        {
            InitializeComponent();

            _vm = new LivingTracingViewModel();
            DataContext = _vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            _vm.Initialize();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _vm.Unload();
        }

    }
}
