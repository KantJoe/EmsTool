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
    /// AppendMqttControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppendMqttControl : UserControl
    {
        public AppendMqttControl()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            UiGlobalContext.CloseRootDialog(false);
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text)
                || ConfigContext.GenericConfig.MqttConfig.EnvironmentConfigs
                    ?.Any(a => a.Environment == TxtName.Text) == true)
            {
                return;
            }

            UiGlobalContext.CloseRootDialog(TxtName.Text);
        }
    }
}
