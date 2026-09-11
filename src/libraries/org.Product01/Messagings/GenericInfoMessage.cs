using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Messagings;
using org.Ui.Messagings;
using org.Utils;
using System.Buffers.Binary;
using System.Linq;

namespace org.Product01.Messagings
{
    public partial class GenericInfoMessage : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Battery1SN))]
        public partial ReadOnlyMemory<ushort> Buff_Holding0000 { get; set; }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AcPvState))]
        [NotifyPropertyChangedFor(nameof(GridState))]
        [NotifyPropertyChangedFor(nameof(LoadState))]
        [NotifyPropertyChangedFor(nameof(BatteryState))]
        [NotifyPropertyChangedFor(nameof(GridPower))]
        public partial ReadOnlyMemory<ushort> Buff_Input0000 { get; set; }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AcpvPower))]
        [NotifyPropertyChangedFor(nameof(GridPower))]
        [NotifyPropertyChangedFor(nameof(LoadPower))]
        [NotifyPropertyChangedFor(nameof(Battery1_ChargePower))]
        [NotifyPropertyChangedFor(nameof(Battery1_DischargePower))]
        [NotifyPropertyChangedFor(nameof(Battery1_Soc))]
        public partial ReadOnlyMemory<ushort> Buff_Input0125 { get; set; }
        public GenericInfoMessage()
        {
            Buff_Input0000 = new ushort[125];
            Buff_Input0125 = new ushort[125];
            Buff_Holding0000 = new ushort[125];
        }

        public string Battery1SN => Buff_Holding0000.GetStringFromBigEndian(59, 15)?.TrimEnd('\0');

        public bool AcPvState => (Buff_Input0000.GetPoint(12) & 0x4) == 4;
        public bool GridState => (Buff_Input0000.GetPoint(12) & 0x1) == 1;
        public bool BatteryState => (Buff_Input0000.GetPoint(12) & 0x2) == 2;
        public bool LoadState => (Buff_Input0000.GetPoint(12) & 0x8) == 8;
        /// <summary>
        /// 交流光伏输入功率
        /// </summary>
        public decimal AcpvPower => Buff_Input0000.GetPointU32(86) / 1000M;
        public decimal GridPower => Buff_Input0000.GetPointU32(63) / 1000M;

        public decimal LoadPower => Buff_Input0125.GetPointU32(212 - 125) / 1000M;

        public decimal Battery1_ChargePower
        {
            get
            {
                var val = Buff_Input0125.GetPoint32(144 - 125);
                return (val < 0 ? Math.Abs(val) : 0) / 1000M;
            }
        }
        public decimal Battery1_DischargePower
        {
            get
            {
                var val = Buff_Input0125.GetPoint32(144 - 125);
                return (val < 0 ? 0 : val) / 1000M;
            }
        }

        public ushort Battery1_Soc => Buff_Input0125.GetPoint(147 - 125);


        public void ReceivedMessage(UshortMessage message)
        {
            switch (message.Mapping)
            {
                case Input00Message.KEY:
                    Buff_Input0000 = message.Buffer;
                    break;
                case Input125Message.KEY:
                    Buff_Input0125 = message.Buffer;
                    break;
                case Holding00Message.KEY:
                    Buff_Holding0000 = message.Buffer;
                    break;

            }
        }
    }
}
