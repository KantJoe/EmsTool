using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Messagings;
using org.Utils;

namespace org.Product01.ViewModels
{
    public partial class Product01SystemSummaryViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ParallelId))]
        [NotifyPropertyChangedFor(nameof(ParallelCount))]
        [NotifyPropertyChangedFor(nameof(ParallelMpptCount))]
        [NotifyPropertyChangedFor(nameof(ParallelGridPower))]
        [NotifyPropertyChangedFor(nameof(ParallelGridFrequency))]
        [NotifyPropertyChangedFor(nameof(ParallelTotalPowerFactor))]
        [NotifyPropertyChangedFor(nameof(GridLineAVoltage))]
        [NotifyPropertyChangedFor(nameof(GridLineBVoltage))]
        [NotifyPropertyChangedFor(nameof(GridLineCVoltage))]
        [NotifyPropertyChangedFor(nameof(ParallelAVoltage))]
        [NotifyPropertyChangedFor(nameof(ParallelACurrent))]
        [NotifyPropertyChangedFor(nameof(ParallelAPower))]
        [NotifyPropertyChangedFor(nameof(ParallelAPowerFactor))]
        [NotifyPropertyChangedFor(nameof(ParallelBVoltage))]
        [NotifyPropertyChangedFor(nameof(ParallelBCurrent))]
        [NotifyPropertyChangedFor(nameof(ParallelBPower))]
        [NotifyPropertyChangedFor(nameof(ParallelBPowerFactor))]
        [NotifyPropertyChangedFor(nameof(ParallelCVoltage))]
        [NotifyPropertyChangedFor(nameof(ParallelCCurrent))]
        [NotifyPropertyChangedFor(nameof(ParallelCPower))]
        [NotifyPropertyChangedFor(nameof(ParallelCPowerFactor))]
        [NotifyPropertyChangedFor(nameof(ParallelCode))]
        public partial ReadOnlyMemory<ushort> Buff_Input0250 { get; set; }

        public override ushort ParallelState => Buff_Input0250.GetPoint(250 - 250);
        public override ushort ParallelIdentity => Buff_Input0250.GetPoint(251 - 250);
        public override ushort ParallelId => Buff_Input0250.GetPoint(252 - 250);
        public override ushort ParallelCount => Buff_Input0250.GetPoint(253 - 250);
        public override string ParallelInvPvPowerText => DTC == 2000 ? NC : Buff_Input0250.GetPointU32(289 - 250).ToString();
        public override ushort ParallelMpptCount => Buff_Input0250.GetPoint(291 - 250);
        public override int ParallelGridPower => Buff_Input0250.GetPoint32(266 - 250);
        public override decimal ParallelGridFrequency => (short)Buff_Input0250.GetPoint(268 - 250) / 100.0M;
        public override decimal ParallelTotalPowerFactor => (short)Buff_Input0250.GetPoint(287 - 250) / 100.0M;
        public override decimal ParallelLineAVoltage => (short)Buff_Input0250.GetPoint(281 - 250) / 10.0M;
        public override decimal ParallelLineBVoltage => (short)Buff_Input0250.GetPoint(282 - 250) / 10.0M;
        public override decimal ParallelLineCVoltage => (short)Buff_Input0250.GetPoint(283 - 250) / 10.0M;
        public override decimal ParallelAVoltage => Buff_Input0250.GetPoint(269 - 250) / 10.0M;
        public override decimal ParallelACurrent => (short)Buff_Input0250.GetPoint(270 - 250);
        public override decimal ParallelAPower => Buff_Input0250.GetPoint32(271 - 250);
        public override decimal ParallelAPowerFactor => (short)Buff_Input0250.GetPoint(284 - 250) / 100.0M;
        public override decimal ParallelBVoltage => Buff_Input0250.GetPoint(273 - 250) / 10.0M;
        public override decimal ParallelBCurrent => (short)Buff_Input0250.GetPoint(274 - 250);
        public override decimal ParallelBPower => Buff_Input0250.GetPoint32(275 - 250);
        public override decimal ParallelBPowerFactor => (short)Buff_Input0250.GetPoint(285 - 250) / 100.0M;
        public override decimal ParallelCVoltage => Buff_Input0250.GetPoint(277 - 250) / 10.0M;
        public override decimal ParallelCCurrent => (short)Buff_Input0250.GetPoint(278 - 250);
        public override decimal ParallelCPower => Buff_Input0250.GetPoint32(279 - 250);
        public override decimal ParallelCPowerFactor => (short)Buff_Input0250.GetPoint(286 - 250) / 100.0M;
        public override string ParallelCode => Buff_Input0250.GetStringFromBigEndian(254 - 250, 12);
        protected override void Input0250Refreshing(UshortMessage message)
        {
            base.Input0250Refreshing(message);

            Buff_Input0250 = message.Buffer;
        }
    }
}
