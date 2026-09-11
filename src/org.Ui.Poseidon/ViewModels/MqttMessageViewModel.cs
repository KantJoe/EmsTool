using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MqttMessageViewModel:ObservableObject
    {
        [ObservableProperty]
        private string _topic;

        [ObservableProperty]
        private int _qos;

        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private int _functionCode;

        [ObservableProperty]
        private int _start;

        [ObservableProperty]
        private int _end;

        [ObservableProperty]
        private string _sn;

        [ObservableProperty]
        private string _createdTime;
    }
}
