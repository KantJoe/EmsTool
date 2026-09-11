using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public partial class TimeSlot48ViewModel:ObservableObject
    {
        [ObservableProperty]
        private string _time;

        [ObservableProperty]
        private decimal _value;
    }
}
