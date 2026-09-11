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
        [NotifyPropertyChangedFor(nameof(InverState))]
        [NotifyPropertyChangedFor(nameof(LoadAVoltage))]
        [NotifyPropertyChangedFor(nameof(LoadACurrent))]
        [NotifyPropertyChangedFor(nameof(LoadAPower))]
        [NotifyPropertyChangedFor(nameof(LoadAPowerFactor))]
        [NotifyPropertyChangedFor(nameof(LoadBVoltage))]
        [NotifyPropertyChangedFor(nameof(LoadBCurrent))]
        [NotifyPropertyChangedFor(nameof(LoadBPower))]
        [NotifyPropertyChangedFor(nameof(LoadBPowerFactor))]
        [NotifyPropertyChangedFor(nameof(LoadCVoltage))]
        [NotifyPropertyChangedFor(nameof(LoadCCurrent))]
        [NotifyPropertyChangedFor(nameof(LoadCPower))]
        [NotifyPropertyChangedFor(nameof(LoadCPowerFactor))]
        [NotifyPropertyChangedFor(nameof(LoadTotalPower))]
        [NotifyPropertyChangedFor(nameof(LoadPowerFactor))]
        [NotifyPropertyChangedFor(nameof(LoadFrequency))]
        [NotifyPropertyChangedFor(nameof(BatteryPower))]
        [NotifyPropertyChangedFor(nameof(BatteryVoltage))]
        [NotifyPropertyChangedFor(nameof(BatterySoc))]
        [NotifyPropertyChangedFor(nameof(BatteryCurrent))]
        [NotifyPropertyChangedFor(nameof(BatteryState))]
        [NotifyPropertyChangedFor(nameof(GridChargePower))]
        [NotifyPropertyChangedFor(nameof(GeneratorPowerText))]
        [NotifyPropertyChangedFor(nameof(SystemEletricPowerText))]
        [NotifyPropertyChangedFor(nameof(SelfUsePowerText))]
        [NotifyPropertyChangedFor(nameof(SolarInverterPowerText))]
        [NotifyPropertyChangedFor(nameof(SystemTimeYear))]
        [NotifyPropertyChangedFor(nameof(SystemTimeMonth))]
        [NotifyPropertyChangedFor(nameof(SystemTimeDayOfMonth))]
        [NotifyPropertyChangedFor(nameof(SystemTimeHour))]
        [NotifyPropertyChangedFor(nameof(SystemTimeMinute))]
        [NotifyPropertyChangedFor(nameof(SystemTimeSecond))]
        [NotifyPropertyChangedFor(nameof(SystemTimeDayOfMonth))]
        public partial ReadOnlyMemory<ushort> Buff_Input0125 { get; set; }

        public override ushort InverState => Buff_Input0125.GetPoint(184 - 125);

        public override decimal LoadAVoltage => Buff_Input0125.GetPoint(125 - 125) / 10.0M;
        public override decimal LoadACurrent => Buff_Input0125.GetPoint(126 - 125) / 10.0M;
        public override uint LoadAPower => Buff_Input0125.GetPointU32(127 - 125);
        public override decimal LoadAPowerFactor => Buff_Input0125.GetPoint(139 - 125) / 100.0M;
        public override decimal LoadBVoltage => Buff_Input0125.GetPoint(129 - 125) / 10.0M;
        public override decimal LoadBCurrent => Buff_Input0125.GetPoint(130 - 125) / 10.0M;
        public override uint LoadBPower => Buff_Input0125.GetPointU32(131 - 125);
        public override decimal LoadBPowerFactor => Buff_Input0125.GetPoint(140 - 125) / 100.0M;
        public override decimal LoadCVoltage => Buff_Input0125.GetPoint(133 - 125) / 10.0M;
        public override decimal LoadCCurrent => Buff_Input0125.GetPoint(134 - 125) / 10.0M;
        public override uint LoadCPower => Buff_Input0125.GetPointU32(135 - 125);
        public override decimal LoadCPowerFactor => Buff_Input0125.GetPoint(141 - 125) / 100.0M;
        public override uint LoadTotalPower => Buff_Input0125.GetPoint(137 - 125);
        public override decimal LoadPowerFactor => Buff_Input0125.GetPoint(142 - 125) / 100.0M;
        public override decimal LoadFrequency => Buff_Input0125.GetPoint(143 - 125) / 100.0M;
        public override int BatteryPower => Buff_Input0125.GetPoint32(144 - 125);
        public override decimal BatteryVoltage => Buff_Input0125.GetPoint(146 - 125) / 10.0M;
        public override ushort BatterySoc => Buff_Input0125.GetPoint(147 - 125);
        public override decimal BatteryCurrent => (short)Buff_Input0125.GetPoint(148 - 125) / 100.0M;
        public override ushort BatteryState => Buff_Input0125.GetPoint(149 - 125);
        public override decimal GridChargePower => Buff_Input0125.GetPoint32(196 - 125) * 1.0M;

        public override string GeneratorPowerText => DTC == 1000 ? NC : Buff_Input0125.GetPointU32(150 - 125).ToString();

        public override string SystemEletricPowerText => DTC == 1000 ? NC : Buff_Input0125.GetPointU32(185 - 125).ToString();
        public override string SelfUsePowerText => DTC == 1000 ? NC : Buff_Input0125.GetPointU32(187 - 125).ToString();
        public override string SolarInverterPowerText => DTC == 1000 ? NC : Buff_Input0125.GetPointU32(198 - 125).ToString();

        public override ushort SystemTimeYear => Buff_Input0125.GetPoint(189 - 125);
        public override ushort SystemTimeMonth => Buff_Input0125.GetPoint(190 - 125);
        public override ushort SystemTimeDayOfMonth => Buff_Input0125.GetPoint(191 - 125);
        public override ushort SystemTimeHour => Buff_Input0125.GetPoint(192 - 125);
        public override ushort SystemTimeMinute => Buff_Input0125.GetPoint(193 - 125);
        public override ushort SystemTimeSecond => Buff_Input0125.GetPoint(194 - 125);
        public override ushort SystemTimeDayOfWeek => Buff_Input0125.GetPoint(195 - 125);
        protected override void Input0125Refreshing(UshortMessage message)
        {
            base.Input0125Refreshing(message);

            Buff_Input0125 = message.Buffer;
        }
    }
}
