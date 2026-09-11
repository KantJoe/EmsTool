using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Messagings;
using System;
using System.Collections.Generic;
using System.Text;
using org.Utils;

namespace org.Product01.ViewModels
{
    public partial class Product01SystemSummaryViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HardwareVersion_4G))]
        [NotifyPropertyChangedFor(nameof(SoftwareVersion1_4G))]
        [NotifyPropertyChangedFor(nameof(SoftwareVersion2_4G))]
        [NotifyPropertyChangedFor(nameof(Imei_4G))]
        [NotifyPropertyChangedFor(nameof(Sim_4G))]
        public partial ReadOnlyMemory<ushort> Buff_Input2125 { get; set; }

        public override string HardwareVersion_4G => Buff_Input2125.GetStringFromBigEndian(2125 - 2125, 10).TrimEnd('\0');
        public override string SoftwareVersion1_4G => Buff_Input2125.GetStringFromBigEndian(2135 - 2125, 10).TrimEnd('\0');
        public override string SoftwareVersion2_4G => Buff_Input2125.GetStringFromBigEndian(2145 - 2125, 15).TrimEnd('\0');
        public override string Imei_4G => Buff_Input2125.GetStringFromBigEndian(2160 - 2125, 10).TrimEnd('\0');
        public override string Sim_4G => Buff_Input2125.GetStringFromBigEndian(2170 - 2125, 15).TrimEnd('\0');

        protected override void Input2125Refreshing(UshortMessage message)
        {
            base.Input2125Refreshing(message);

            Buff_Input2125 = message.Buffer;
        }
    }
}
