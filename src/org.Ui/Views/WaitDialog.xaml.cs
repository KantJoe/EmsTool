using org.Ui.MultiLanguage;
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
    /// WaitCancelDialog.xaml 的交互逻辑
    /// </summary>
    public partial class WaitDialog : UserControl
    {
        public WaitDialog(string buttonText="取消")
        {
            InitializeComponent();
            BtnCancel.Content = MultiLang.GetString(buttonText);
        }
    }
}
