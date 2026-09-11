using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace org.Ui.ViewModels
{
    public partial class EndPointViewModel:ObservableObject
    {
        public EmsDevice Device { get; private set; }

        public CommunicateType CommunicateType
        {
            get => Device?.CommunicateType ?? CommunicateType.None;
            set
            {
                var v = Device?.CommunicateType ?? CommunicateType.None;
                if (Device is not null && SetProperty(ref v, value, nameof(CommunicateType)))
                {
                    Device.CommunicateType = v;
                }
            }
        }


        public EndPointViewModel(EmsDevice device)
        {
            Device = device;
        }

    }
}
