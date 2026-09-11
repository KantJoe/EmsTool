using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Messagings;
using org.Utils;

namespace org.Product01.ViewModels
{
    public partial class Product01SystemSummaryViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SystemState))]
        [NotifyPropertyChangedFor(nameof(GridConnected))]
        [NotifyPropertyChangedFor(nameof(Onlined))]
        [NotifyPropertyChangedFor(nameof(EmsState))]
        [NotifyPropertyChangedFor(nameof(FanRpm))]
        [NotifyPropertyChangedFor(nameof(ConfigFileCode))]
        [NotifyPropertyChangedFor(nameof(StateOfCharge))]
        [NotifyPropertyChangedFor(nameof(Humidity01))]
        [NotifyPropertyChangedFor(nameof(Humidity02))]
        [NotifyPropertyChangedFor(nameof(Temperature01))]
        [NotifyPropertyChangedFor(nameof(Temperature02))]
        [NotifyPropertyChangedFor(nameof(Temperature03))]
        [NotifyPropertyChangedFor(nameof(Temperature04))]
        [NotifyPropertyChangedFor(nameof(EmsFirmware))]
        [NotifyPropertyChangedFor(nameof(BmsFirmware))]
        [NotifyPropertyChangedFor(nameof(HmiFirmware))]
        [NotifyPropertyChangedFor(nameof(BmsOnlineState))]
        [NotifyPropertyChangedFor(nameof(FireOnlineState))]
        [NotifyPropertyChangedFor(nameof(PcsOnlineState))]
        [NotifyPropertyChangedFor(nameof(DcdcOnlineState))]
        [NotifyPropertyChangedFor(nameof(MpptOnlineState))]
        [NotifyPropertyChangedFor(nameof(StsOnlineState))]
        [NotifyPropertyChangedFor(nameof(ElectricOnlineState))]
        [NotifyPropertyChangedFor(nameof(AirConditionOnlineState))]
        [NotifyPropertyChangedFor(nameof(LiquidOnlineState))]
        [NotifyPropertyChangedFor(nameof(HmiOnlineState))]
        [NotifyPropertyChangedFor(nameof(PcOnlineState))]
        [NotifyPropertyChangedFor(nameof(GpsOnlineState))]
        [NotifyPropertyChangedFor(nameof(FourGOnlineState))]
        [NotifyPropertyChangedFor(nameof(WifiOnlineState))]
        [NotifyPropertyChangedFor(nameof(BluetoothOnlineState))]
        [NotifyPropertyChangedFor(nameof(Lan1OnlineState))]
        [NotifyPropertyChangedFor(nameof(Lan2OnlineState))]
        [NotifyPropertyChangedFor(nameof(HumidityAndTempOnlineState))]
        [NotifyPropertyChangedFor(nameof(GridConnectedOnlineState))]
        [NotifyPropertyChangedFor(nameof(GridPower))]
        [NotifyPropertyChangedFor(nameof(GridFrequency))]
        [NotifyPropertyChangedFor(nameof(GridAVoltage))]
        [NotifyPropertyChangedFor(nameof(GridACurrent))]
        [NotifyPropertyChangedFor(nameof(GridAPower))]
        [NotifyPropertyChangedFor(nameof(GridBVoltage))]
        [NotifyPropertyChangedFor(nameof(GridBCurrent))]
        [NotifyPropertyChangedFor(nameof(GridBPower))]
        [NotifyPropertyChangedFor(nameof(GridCVoltage))]
        [NotifyPropertyChangedFor(nameof(GridCCurrent))]
        [NotifyPropertyChangedFor(nameof(GridCPower))]
        [NotifyPropertyChangedFor(nameof(GridLineAVoltage))]
        [NotifyPropertyChangedFor(nameof(GridLineBVoltage))]
        [NotifyPropertyChangedFor(nameof(GridLineCVoltage))]
        [NotifyPropertyChangedFor(nameof(GridAPowerFactor))]
        [NotifyPropertyChangedFor(nameof(GridBPowerFactor))]
        [NotifyPropertyChangedFor(nameof(GridCPowerFactor))]
        [NotifyPropertyChangedFor(nameof(GridTotalPowerFactor))]
        [NotifyPropertyChangedFor(nameof(GridState))]
        [NotifyPropertyChangedFor(nameof(PvInputPower))]
        [NotifyPropertyChangedFor(nameof(PvMpptCount))]
        [NotifyPropertyChangedFor(nameof(Pv1VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv1CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv2VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv2CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv3VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv3CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv4VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv4CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv5VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv5CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv6VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv6CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv7VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv7CurrentText))]
        [NotifyPropertyChangedFor(nameof(Pv8VoltageText))]
        [NotifyPropertyChangedFor(nameof(Pv8CurrentText))]
        public partial ReadOnlyMemory<ushort> Buff_Input0000 { get; set; }

        public override ushort SystemState => Buff_Input0000.GetPoint(0);
        public override ushort RunningMode => Buff_Input0000.GetPoint(1);
        public override bool BmsOnlineState => (Buff_Input0000.GetPoint(2) & 1) == 1;
        public override bool FireOnlineState => (Buff_Input0000.GetPoint(2) & 2) == 2;
        public override bool PcsOnlineState => (Buff_Input0000.GetPoint(2) & 4) == 4;
        public override bool DcdcOnlineState => (Buff_Input0000.GetPoint(2) & 8) == 8;
        public override bool MpptOnlineState => (Buff_Input0000.GetPoint(2) & 16) == 16;
        public override bool StsOnlineState => (Buff_Input0000.GetPoint(2) & 32) == 32;
        public override bool ElectricOnlineState => (Buff_Input0000.GetPoint(2) & 64) == 64;
        public override bool AirConditionOnlineState => (Buff_Input0000.GetPoint(2) & 128) == 128;
        public override bool LiquidOnlineState => (Buff_Input0000.GetPoint(2) & 256) == 256;
        public override bool HmiOnlineState => (Buff_Input0000.GetPoint(2) & 512) == 512;
        public override bool PcOnlineState => (Buff_Input0000.GetPoint(2) & 1024) == 1024;
        public override bool GpsOnlineState => (Buff_Input0000.GetPoint(2) & 2048) == 2048;
        public override bool FourGOnlineState => (Buff_Input0000.GetPoint(2) & 4096) == 4096;
        public override bool WifiOnlineState => (Buff_Input0000.GetPoint(2) & 8192) == 8192;
        public override bool BluetoothOnlineState => (Buff_Input0000.GetPoint(2) & 16384) == 16384;
        public override bool Lan1OnlineState => (Buff_Input0000.GetPoint(2) & 32768) == 32768;
        public override bool Lan2OnlineState => (Buff_Input0000.GetPoint(3) & 1) == 1;
        public override bool HumidityAndTempOnlineState => (Buff_Input0000.GetPoint(3) & 2) == 2;
        public override bool GridConnectedOnlineState => (Buff_Input0000.GetPoint(3) & 4) == 4;

        public override ushort Onlined => Buff_Input0000.GetPoint(4);
        public override int StateOfCharge => Buff_Input0000.GetPoint32(10);

        public override ushort GridConnected => Buff_Input0000.GetPoint(13);
        public override ushort EmsState => Buff_Input0000.GetPoint(14);
        public override ushort FanRpm => Buff_Input0000.GetPoint(15);
        public override ushort ConfigFileCode => Buff_Input0000.GetPoint(16);
        public override decimal Humidity01 => Buff_Input0000.GetPoint(17) / 10.0M;
        public override decimal Humidity02 => Buff_Input0000.GetPoint(18) / 10.0M;

        public override decimal Temperature01 => (short)Buff_Input0000.GetPoint(19) / 10M;
        public override decimal Temperature02 => (short)Buff_Input0000.GetPoint(20) / 10M;
        public override decimal Temperature03 => (short)Buff_Input0000.GetPoint(21) / 10M;
        public override decimal Temperature04 => (short)Buff_Input0000.GetPoint(22) / 10M;

        public override string EmsFirmware => Buff_Input0000.GetStringFromBigEndian(33, 10)?.TrimEnd('\0');

        public override string BmsFirmware => Buff_Input0000.GetStringFromBigEndian(43, 10)?.TrimEnd('\0');

        public override string HmiFirmware => Buff_Input0000.GetStringFromBigEndian(53, 10)?.TrimEnd('\0');

        public override int GridPower => Buff_Input0000.GetPoint32(63);
        public override decimal GridFrequency => Buff_Input0000.GetPoint(65) / 100.0M;
        public override decimal GridAVoltage => Buff_Input0000.GetPoint(66) / 10.0M;
        public override decimal GridACurrent => (ushort)Buff_Input0000.GetPoint(67) / 10.0M;
        public override decimal GridAPower => Buff_Input0000.GetPoint32(68);
        public override decimal GridBVoltage => Buff_Input0000.GetPoint(70) / 10.0M;
        public override decimal GridBCurrent => (ushort)Buff_Input0000.GetPoint(71) / 10.0M;
        public override decimal GridBPower => Buff_Input0000.GetPoint32(72);
        public override decimal GridCVoltage => Buff_Input0000.GetPoint(74) / 10.0M;
        public override decimal GridCCurrent => (ushort)Buff_Input0000.GetPoint(75) / 10.0M;
        public override decimal GridCPower => Buff_Input0000.GetPoint32(76);
        public override decimal GridLineAVoltage => Buff_Input0000.GetPoint(78) / 10.0M;
        public override decimal GridLineBVoltage => Buff_Input0000.GetPoint(79) / 10.0M;
        public override decimal GridLineCVoltage => Buff_Input0000.GetPoint(80) / 10.0M;
        public override decimal GridAPowerFactor => (short)Buff_Input0000.GetPoint(81) / 100.0M;
        public override decimal GridBPowerFactor => (short)Buff_Input0000.GetPoint(82) / 100.0M;
        public override decimal GridCPowerFactor => (short)Buff_Input0000.GetPoint(83) / 100.0M;
        public override decimal GridTotalPowerFactor => (short)Buff_Input0000.GetPoint(84) / 100.0M;
        public override ushort GridState => Buff_Input0000.GetPoint(85);

        public override uint PvInputPower => Buff_Input0000.GetPointU32(86);
        public override ushort PvMpptCount => Buff_Input0000.GetPoint(88);
        public override string Pv1VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(89) / 10.0M).ToString();
        public override string Pv1CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(90) / 100.0M).ToString();
        public override string Pv2VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(91) / 10.0M).ToString();
        public override string Pv2CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(92) / 100.0M).ToString();
        public override string Pv3VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(93) / 10.0M).ToString();
        public override string Pv3CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(94) / 100.0M).ToString();
        public override string Pv4VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(95) / 10.0M).ToString();
        public override string Pv4CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(96) / 100.0M).ToString();
        public override string Pv5VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(97) / 10.0M).ToString();
        public override string Pv5CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(98) / 100.0M).ToString();
        public override string Pv6VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(99) / 10.0M).ToString();
        public override string Pv6CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(100) / 100.0M).ToString();
        public override string Pv7VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(101) / 10.0M).ToString();
        public override string Pv7CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(102) / 100.0M).ToString();
        public override string Pv8VoltageText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(103) / 10.0M).ToString();
        public override string Pv8CurrentText => DTC == 1000 ? NC : (Buff_Input0000.GetPoint(104) / 100.0M).ToString();
        

        protected override void Input0000Refreshing(UshortMessage message)
        {
            base.Input0000Refreshing(message);

            Buff_Input0000 = message.Buffer;
        }
    }
}
