using org.Models;
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
    /// EditPointAlertDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditPointAlertDialog : UserControl
    {
        private EmsDevicePointAlert _point;
        public EditPointAlertDialog(EmsDevicePointAlert point)
        {
            InitializeComponent();

            _point = point;
            TxtFunctionCode.Text = ((int)point.FunctionCode).ToString().PadLeft(2, '0');
            TxtAddress.Text = point.StartAddress.ToString();
            TxtCount.Text = point.PointCount.ToString();
            TxtThreshold.Text = point.ThresholdRangeValue.ToString();
            CbCondition.SelectedIndex = (int)point.Condition;
            CbTimes.SelectedIndex = (int)point.AlertTimes;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (CbCondition.SelectedIndex == (int)AlertCondition.Between
                || CbCondition.SelectedIndex == (int)AlertCondition.NotBetween)
            {
                var vals = TxtThreshold.Text.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (vals.Length != 2
                    || !ushort.TryParse(vals[0], out _)
                    || !ushort.TryParse(vals[0], out _))
                {
                    return;
                }
            }
            else if (CbCondition.SelectedIndex != (int)AlertCondition.Changed&&!ushort.TryParse(TxtThreshold.Text, out _))
            {
                return;
            }

            _point.ThresholdRangeValue = TxtThreshold.Text;
            _point.Condition = (AlertCondition)CbCondition.SelectedIndex;
            _point.AlertTimes = (AlertTimes)CbTimes.SelectedIndex;
            UiGlobalContext.CloseRootDialog(_point);
        }

        private void CbCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCondition.SelectedIndex == (int)AlertCondition.Changed)
            {
                CbTimes.SelectedIndex = (int)AlertTimes.Always;
                CbTimes.IsEnabled = false;
                TxtThreshold.IsEnabled = false;
            }
            else
            {
                CbTimes?.IsEnabled = true;
                TxtThreshold?.IsEnabled = true;
            }
        }
    }
}
