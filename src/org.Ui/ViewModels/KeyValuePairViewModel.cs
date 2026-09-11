using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public partial class KeyValuePairViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _key;

        [ObservableProperty]
        private string _value;

        [ObservableProperty]
        private bool _isChecked;

        public KeyValuePairViewModel(string key, string value, bool isChecked)
        {
            Key = key;
            Value = value;
            IsChecked = isChecked;
        }
    }
}
