using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public partial class ComboxItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _display;
        [ObservableProperty]
        private string _value;

    }
}
