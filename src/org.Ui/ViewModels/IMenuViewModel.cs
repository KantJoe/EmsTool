using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public interface IMenuViewModel
    {
        PointMapBaseViewModel PointMapInitialize();

        SystemSettingViewModelBase SystemSettingViewModelInitialize();
    }
}
