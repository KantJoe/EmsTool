using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MqttTopicViewModel:ObservableObject
    {
        [ObservableProperty]
        private string _topic;

        [ObservableProperty]
        private int _qos;

        public MqttTopicViewModel(string topic,int qos)
        {
            Topic = topic;
            Qos = qos;
        }
    }
}
