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
        [NotifyPropertyChangedFor(nameof(PsfvGroupValidTotalPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupValidPeakPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupValidSharpPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupValidFlatPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupValidVallyPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveValidTotalPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveValidPeakPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveValidSharpPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveValidFlatPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveValidVallyPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeValidTotalPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeValidPeakPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeValidSharpPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeValidFlatPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeValidVallyPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupInvalidTotalPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupInvalidPeakPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupInvalidSharpPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupInvalidFlatPower))]
        [NotifyPropertyChangedFor(nameof(PsfvGroupInvalidVallyPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveInvalidTotalPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveInvalidPeakPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveInvalidSharpPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveInvalidFlatPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPositiveInvalidVallyPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeInvalidTotalPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeInvalidPeakPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeInvalidSharpPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeInvalidFlatPower))]
        [NotifyPropertyChangedFor(nameof(PsfvOppositeInvalidVallyPower))]
        [NotifyPropertyChangedFor(nameof(PsfvPeakUnitPrice))]
        [NotifyPropertyChangedFor(nameof(PsfvSharpUnitPrice))]
        [NotifyPropertyChangedFor(nameof(PsfvFlatUnitPrice))]
        [NotifyPropertyChangedFor(nameof(PsfvVallyUnitPrice))]
        public partial ReadOnlyMemory<ushort> Buff_Input1875 { get; set; }

        public override decimal PsfvGroupValidTotalPower => Buff_Input1875.GetPoint32(1875 - 1875) / 10.0M;
        public override decimal PsfvGroupValidPeakPower => Buff_Input1875.GetPoint32(1877 - 1875) / 10.0M;
        public override decimal PsfvGroupValidSharpPower => Buff_Input1875.GetPoint32(1879 - 1875) / 10.0M;
        public override decimal PsfvGroupValidFlatPower => Buff_Input1875.GetPoint32(1881 - 1875) / 10.0M;
        public override decimal PsfvGroupValidVallyPower => Buff_Input1875.GetPoint32(1883 - 1875) / 10.0M;

        public override decimal PsfvPositiveValidTotalPower => Buff_Input1875.GetPoint32(1885 - 1875) / 10.0M;
        public override decimal PsfvPositiveValidPeakPower => Buff_Input1875.GetPoint32(1887 - 1875) / 10.0M;
        public override decimal PsfvPositiveValidSharpPower => Buff_Input1875.GetPoint32(1889 - 1875) / 10.0M;
        public override decimal PsfvPositiveValidFlatPower => Buff_Input1875.GetPoint32(1891 - 1875) / 10.0M;
        public override decimal PsfvPositiveValidVallyPower => Buff_Input1875.GetPoint32(1893 - 1875) / 10.0M;

        public override decimal PsfvOppositeValidTotalPower => Buff_Input1875.GetPoint32(1895 - 1875) / 10.0M;
        public override decimal PsfvOppositeValidPeakPower => Buff_Input1875.GetPoint32(1897 - 1875) / 10.0M;
        public override decimal PsfvOppositeValidSharpPower => Buff_Input1875.GetPoint32(1899 - 1875) / 10.0M;
        public override decimal PsfvOppositeValidFlatPower => Buff_Input1875.GetPoint32(1901 - 1875) / 10.0M;
        public override decimal PsfvOppositeValidVallyPower => Buff_Input1875.GetPoint32(1903 - 1875) / 10.0M;

        public override decimal PsfvGroupInvalidTotalPower => Buff_Input1875.GetPoint32(1905 - 1875) / 10.0M;
        public override decimal PsfvGroupInvalidPeakPower => Buff_Input1875.GetPoint32(1907 - 1875) / 10.0M;
        public override decimal PsfvGroupInvalidSharpPower => Buff_Input1875.GetPoint32(1909 - 1875) / 10.0M;
        public override decimal PsfvGroupInvalidFlatPower => Buff_Input1875.GetPoint32(1911 - 1875) / 10.0M;
        public override decimal PsfvGroupInvalidVallyPower => Buff_Input1875.GetPoint32(1913 - 1875) / 10.0M;

        public override decimal PsfvPositiveInvalidTotalPower => Buff_Input1875.GetPoint32(1915 - 1875) / 10.0M;
        public override decimal PsfvPositiveInvalidPeakPower => Buff_Input1875.GetPoint32(1917 - 1875) / 10.0M;
        public override decimal PsfvPositiveInvalidSharpPower => Buff_Input1875.GetPoint32(1919 - 1875) / 10.0M;
        public override decimal PsfvPositiveInvalidFlatPower => Buff_Input1875.GetPoint32(1921 - 1875) / 10.0M;
        public override decimal PsfvPositiveInvalidVallyPower => Buff_Input1875.GetPoint32(1923 - 1875) / 10.0M;

        public override decimal PsfvOppositeInvalidTotalPower => Buff_Input1875.GetPoint32(1925 - 1875) / 10.0M;
        public override decimal PsfvOppositeInvalidPeakPower => Buff_Input1875.GetPoint32(1927 - 1875) / 10.0M;
        public override decimal PsfvOppositeInvalidSharpPower => Buff_Input1875.GetPoint32(1929 - 1875) / 10.0M;
        public override decimal PsfvOppositeInvalidFlatPower => Buff_Input1875.GetPoint32(1931 - 1875) / 10.0M;
        public override decimal PsfvOppositeInvalidVallyPower => Buff_Input1875.GetPoint32(1933 - 1875) / 10.0M;

        public override decimal PsfvPeakUnitPrice => Buff_Input1875.GetPoint32(1935 - 1875) / 100000000.0M;
        public override decimal PsfvSharpUnitPrice => Buff_Input1875.GetPoint32(1937 - 1875) / 100000000.0M;
        public override decimal PsfvFlatUnitPrice => Buff_Input1875.GetPoint32(1939 - 1875) / 100000000.0M;
        public override decimal PsfvVallyUnitPrice => Buff_Input1875.GetPoint32(1941 - 1875) / 100000000.0M;

        protected override void Input1875Refreshing(UshortMessage message)
        {
            base.Input1875Refreshing(message);

            Buff_Input1875 = message.Buffer;
        }
    }
}
