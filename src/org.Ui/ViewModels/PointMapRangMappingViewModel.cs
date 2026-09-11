using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public partial class PointMapRangMappingViewModel : ObservableObject
    {
        public PointMapRangMapping Mapping { get; set; }

        public int FunctionCode
        {
            get => (int)(Mapping?.FunctionCode ?? ModbusFunctionCode.ReadInputRegisters);
            set
            {
                var v = (int)Mapping.FunctionCode;
                if (SetProperty(ref v, value))
                {
                    Mapping.FunctionCode = (ModbusFunctionCode)v;
                }
            }
        }

        public int StartAddress
        {
            get => Mapping?.StartAddress ?? 0;
            set
            {
                var v = Mapping.StartAddress;
                if (SetProperty(ref v, value))
                {
                    Mapping.StartAddress = v;
                }
            }
        }

        public int MapLimitation
        {
            get => Mapping?.MapLimitation ?? 0;
            set
            {
                var v = Mapping.MapLimitation;
                if (SetProperty(ref v, value))
                {
                    Mapping.MapLimitation = v;
                }
            }
        }

        public string GroupName
        {
            get => Mapping?.GroupName;
            set
            {
                var v = Mapping.GroupName;
                if (SetProperty(ref v, value))
                {
                    Mapping.GroupName = v;
                }
            }
        }

        public int PointCount
        {
            get => Mapping?.PointCount ?? 0;
            set
            {
                var v = Mapping.PointCount;
                if (SetProperty(ref v, value))
                {
                    Mapping.PointCount = v;
                }
            }
        }

        public string Display => $"{((int)Mapping.FunctionCode).ToString().PadLeft(2,'0')}_{Mapping.GroupName}:[{Mapping.StartAddress}-{Mapping.StartAddress+Mapping.PointCount-1}]";

        public PointMapRangMappingViewModel(PointMapRangMapping map)
        {
            Mapping = map;
        }
    }
}
