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
    /// EditPointDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditPointDialog : UserControl
    {
        private PointViewModel _point;
        public EditPointDialog(PointViewModel point)
        {
            InitializeComponent();

            _point = point;
            TxtEms.Text = point.Key;
            TxtFunctionCode.Text = point.FunctionCode.ToString().PadLeft(2, '0');
            NumStartAddress.Value = point.Address;
            TxtValue.Text = point.CurrentValue.ToString();
        }

        private void CbDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _point.DateType = CbDataType.SelectionBoxItem.ToString();
            _point.PointCount = NumCount.Value;
            _point.NextValue = TxtValue.Text;

            UiGlobalContext.CloseRootDialog(_point);
        }
    }
}
