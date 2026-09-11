using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MqttWritableDataViewModel : ObservableObject
    {
        [ObservableProperty]
        private ushort _address;

        private ushort _value;
        public ushort Value
        {
            get => _value;
            set
            {
                if (SetProperty(ref _value, value))
                {
                    StrValue = "0x" + value.ToString("x4");
                }
            }
        }

        [ObservableProperty]
        private string _strValue;

        public MqttWritableDataViewModel()
        {
            Value = 0;
        }
    }
}
