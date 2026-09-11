using org.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace org.Ui.Views
{
    public interface IMenuControl
    {
        UserControl CreateDeviceTopologyControl(object obj);

        UserControl CreateSystemSummaryControl(object obj);
    }
}
