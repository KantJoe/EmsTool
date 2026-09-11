using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Communication;
using org.Communication.Extensions;
using org.Models;
using org.Ui;
using org.Ui.ViewModels;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Modbus;

namespace org.Communication.ViewModels
{
    public partial class EmsNetworkEndpointViewModel : EndPointViewModel
    {
        [ObservableProperty]
        private ConnectionOptionsViewModel _options;

        [ObservableProperty]
        private ObservableCollection<string> _protocols;

        public string SelectedProtocol
        {
            get => Device?.EmsProtocol ?? EmsProtocol.Product01;
            set
            {
                var v = Device?.EmsProtocol ?? EmsProtocol.Product01;
                if (Device is not null && SetProperty(ref v, value, nameof(EmsProtocol)))
                {
                    Device.EmsProtocol = v;
                }
            }
        }

        public int OrderNumber
        {
            get => Device?.OrderNumber ?? 0;
            set
            {
                var v = Device?.OrderNumber ?? 0;
                if (Device is not null && SetProperty(ref v, value, nameof(OrderNumber)))
                {
                    Device.OrderNumber = v;
                }
            }
        }

        public string Alias
        {
            get => Device?.Alias;
            set
            {
                var v = Device.Alias ?? string.Empty;
                if (Device is not null && SetProperty(ref v, value, nameof(Alias)))
                {
                    Device.Alias = v;
                }
            }
        }

        public IConnectClient ModbusObject { get; set; }

        public EmsNetworkEndpointViewModel(EmsDevice device)
            : base(device)
        {

            Protocols =
                [EmsProtocol.Product01];
            Options = new ConnectionOptionsViewModel(device.ConnectionOptions);
        }


        public static EmsConnectionOption NewRtuOptions(int orderNumber,byte slaveId=1)
        {
            return EmsConnectionOption.CreateDefaultRtuOptions(orderNumber,slaveId);
        }

        public static EmsConnectionOption NewTcpOptions(int orderNumber)
        {
            return EmsConnectionOption.CreateDefaultTcpOptions(orderNumber);
        }

        public void SetModbusObject(IConnectClient adapter)
        {
            this.ModbusObject = adapter;
        }
    }
}
