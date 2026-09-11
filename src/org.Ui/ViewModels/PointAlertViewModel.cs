using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using org.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace org.Ui.ViewModels
{
    public partial class PointAlertViewModel : ObservableObject
    {
        public EmsDevicePointAlert Point { get; private set; }

        public string FunctionCode => ((int)Point?.FunctionCode).ToString().PadLeft(2,'0');

        [ObservableProperty]
        private string _modbusKey;

        [ObservableProperty]
        private ushort _currentValue;

        [ObservableProperty]
        private bool _overLimited;


        [ObservableProperty]
        private ObservableCollection<string> _alertTimes;

        [ObservableProperty]
        private Brush _background;
        public PointAlertViewModel(EmsDevicePointAlert point, string key)
        {
            Point = point;
            ModbusKey = key;
            AlertTimes = [];
        }

    }
}
