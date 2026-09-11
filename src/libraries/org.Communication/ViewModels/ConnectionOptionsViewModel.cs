using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Communication.ViewModels
{
    public partial class ConnectionOptionsViewModel : ObservableObject
    {
        private EmsConnectionOption _options;

        #region modbus rtu
        public byte SlaveId
        {
            get => _options?.SlaveId ?? 1;
            set
            {
                var v = _options?.SlaveId ?? 1;
                if (_options is not null && SetProperty(ref v, value, nameof(SlaveId)))
                {
                    _options.SlaveId = v;

                    OnPropertyChanged("");
                }
            }
        }
        public string PortName
        {
            get => _options?.PortName;
            set
            {
                var v = _options.PortName ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(PortName)))
                {
                    _options.PortName = v;


                    OnPropertyChanged("");
                }
            }
        }
        public string DataBits
        {
            get => _options?.DataBits;
            set
            {
                var v = _options.DataBits ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(DataBits)))
                {
                    _options.DataBits = v;

                    OnPropertyChanged("");
                }
            }
        }
        public string BaudRate
        {
            get => _options?.BaudRate;
            set
            {
                var v = _options.BaudRate ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(BaudRate)))
                {
                    _options.BaudRate = v;

                    OnPropertyChanged("");
                }
            }
        }
        public string Parity
        {
            get => _options?.Parity;
            set
            {
                var v = _options.Parity ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(Parity)))
                {
                    _options.Parity = v;

                    OnPropertyChanged("");
                }
            }
        }
        public string StopBits
        {
            get => _options?.StopBits;
            set
            {
                var v = _options.StopBits ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(StopBits)))
                {
                    _options.StopBits = v;

                    OnPropertyChanged("");
                }
            }
        }
        #endregion

        #region modbus tcp
        public string Ip
        {
            get => _options?.Ip;
            set
            {
                var v = _options.Ip ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(Ip)))
                {
                    _options.Ip = v;

                    OnPropertyChanged("");
                }
            }
        }
        public string Port
        {
            get => _options?.Port;
            set
            {
                var v = _options.Port ?? string.Empty;
                if (_options is not null && SetProperty(ref v, value, nameof(Port)))
                {
                    _options.Port = v;

                    OnPropertyChanged("");
                }
            }
        }
        #endregion

        public int OrderNumber
        {
            get => _options?.OrderNumber ?? 0;
            set
            {
                var v = _options?.OrderNumber ?? 0;
                if (_options is not null && SetProperty(ref v, value, nameof(OrderNumber)))
                {
                    _options.OrderNumber = v;

                    OnPropertyChanged("");
                }
            }
        }

        public ConnectionOptionsViewModel(EmsConnectionOption option)
        {
            _options = option;
        }

        public EmsConnectionOption GetOptions()
        {
            return _options;
        }
    }
}
