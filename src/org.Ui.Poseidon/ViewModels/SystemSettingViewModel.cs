using CommunityToolkit.Mvvm.ComponentModel;
using org.Communication;
using org.Communication.Extensions;
using org.Ui.MultiLanguage;
using org.Ui.ViewModels;
using org.Utils;
using org.Utils.Global;
using System.Collections.ObjectModel;
using System.Net;

namespace org.Ui.Poseidon.ViewModels
{
    /// <summary>
    /// product01
    /// StackX,
    /// </summary>
    public partial class SystemSettingViewModel : SystemSettingViewModelBase
    {
        public SystemSettingViewModel() : base()
        {
            MultiLang.OnLanguageChanged += MultiLang_OnLanguageChanged;
            SystemPowerStatusesInitialize();
            LcdLanguagesInitialize();
            HmiBaudRatesInitialize();
            DebugabledsInitialize();
            DebugProtectablesInitialize();

            Buff_Holding0000 = new ushort[55];
        }

        #region system param01
        [ObservableProperty]
        private bool _systemParam01Queried;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SystemPowerStatus))]
        [NotifyPropertyChangedFor(nameof(Soc))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_31_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_32_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_33_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_34_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_35_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_36_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_37_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_38_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_39_Bit15))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit3))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit4))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit5))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit6))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit7))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit8))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit9))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit10))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit11))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit12))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit13))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit14))]
        [NotifyPropertyChangedFor(nameof(Buff_40_Bit15))]
        [NotifyPropertyChangedFor(nameof(YearOfTime))]
        [NotifyPropertyChangedFor(nameof(MonthOfTime))]
        [NotifyPropertyChangedFor(nameof(DayOfTime))]
        [NotifyPropertyChangedFor(nameof(HourOfTime))]
        [NotifyPropertyChangedFor(nameof(MinuteOfTime))]
        [NotifyPropertyChangedFor(nameof(SecondOfTime))]
        [NotifyPropertyChangedFor(nameof(WeekDayOfTime))]
        [NotifyPropertyChangedFor(nameof(LcdLanguage))]
        [NotifyPropertyChangedFor(nameof(HmiBaudRate))]
        [NotifyPropertyChangedFor(nameof(HmiAddress))]
        [NotifyPropertyChangedFor(nameof(UpgradeFirmwareAddress))]
        [NotifyPropertyChangedFor(nameof(Buff_52_Bit0))]
        [NotifyPropertyChangedFor(nameof(Buff_52_Bit1))]
        [NotifyPropertyChangedFor(nameof(Buff_52_Bit2))]
        [NotifyPropertyChangedFor(nameof(Buff_52_Bit3))]
        [NotifyPropertyChangedFor(nameof(Debugabled))]
        [NotifyPropertyChangedFor(nameof(DebugProtectable))]
        public partial ushort[] Buff_Holding0000 { get; set; }

        public ushort SystemPowerStatus
        {
            get
            {
                return Buff_Holding0000.GetPoint(1);
            }
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 1, 1);
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComboxItemViewModel> _systemPowerStatuses;
        private void SystemPowerStatusesInitialize()
        {
            SystemPowerStatuses = [..
                new[] {
                    new ComboxItemViewModel { Value = "0", Display = MultiLang.GetString("Off") },
                    new ComboxItemViewModel { Value = "1", Display = MultiLang.GetString("On") }
                }];
        }

        public decimal Soc
        {
            get => Buff_Holding0000.GetPoint(7) / 10.0M;
            set
            {
                Array.Copy(new[] { (ushort)(value * 10) }, 0, Buff_Holding0000, 7, 1);
            }
        }

        #region 31
        public bool Buff_31_Bit0
        {
            get => (Buff_Holding0000.GetPoint(31) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 0) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit1
        {
            get => (Buff_Holding0000.GetPoint(31) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 1) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit2
        {
            get => (Buff_Holding0000.GetPoint(31) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 2) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit3
        {
            get => (Buff_Holding0000.GetPoint(31) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 3) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit4
        {
            get => (Buff_Holding0000.GetPoint(31) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 4) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit5
        {
            get => (Buff_Holding0000.GetPoint(31) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 5) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit6
        {
            get => (Buff_Holding0000.GetPoint(31) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 6) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit7
        {
            get => (Buff_Holding0000.GetPoint(31) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 7) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit8
        {
            get => (Buff_Holding0000.GetPoint(31) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 8) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit9
        {
            get => (Buff_Holding0000.GetPoint(31) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 9) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit10
        {
            get => (Buff_Holding0000.GetPoint(31) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 10) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit11
        {
            get => (Buff_Holding0000.GetPoint(31) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 11) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit12
        {
            get => (Buff_Holding0000.GetPoint(31) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 12) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit13
        {
            get => (Buff_Holding0000.GetPoint(31) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 13) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit14
        {
            get => (Buff_Holding0000.GetPoint(31) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 14) }, 0, Buff_Holding0000, 31, 1);
            }
        }

        public bool Buff_31_Bit15
        {
            get => (Buff_Holding0000.GetPoint(31) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[31].SetBit(value, 15) }, 0, Buff_Holding0000, 31, 1);
            }
        }
        #endregion

        #region 32
        public bool Buff_32_Bit0
        {
            get => (Buff_Holding0000.GetPoint(32) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 0) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit1
        {
            get => (Buff_Holding0000.GetPoint(32) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 1) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit2
        {
            get => (Buff_Holding0000.GetPoint(32) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 2) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit3
        {
            get => (Buff_Holding0000.GetPoint(32) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 3) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit4
        {
            get => (Buff_Holding0000.GetPoint(32) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 4) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit5
        {
            get => (Buff_Holding0000.GetPoint(32) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 5) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit6
        {
            get => (Buff_Holding0000.GetPoint(32) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 6) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit7
        {
            get => (Buff_Holding0000.GetPoint(32) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 7) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit8
        {
            get => (Buff_Holding0000.GetPoint(32) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 8) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit9
        {
            get => (Buff_Holding0000.GetPoint(32) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 9) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit10
        {
            get => (Buff_Holding0000.GetPoint(32) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 10) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit11
        {
            get => (Buff_Holding0000.GetPoint(32) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 11) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit12
        {
            get => (Buff_Holding0000.GetPoint(32) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 12) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit13
        {
            get => (Buff_Holding0000.GetPoint(32) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 13) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit14
        {
            get => (Buff_Holding0000.GetPoint(32) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 14) }, 0, Buff_Holding0000, 32, 1);
            }
        }

        public bool Buff_32_Bit15
        {
            get => (Buff_Holding0000.GetPoint(32) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[32].SetBit(value, 15) }, 0, Buff_Holding0000, 32, 1);
            }
        }
        #endregion

        #region 33
        public bool Buff_33_Bit0
        {
            get => (Buff_Holding0000.GetPoint(33) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 0) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit1
        {
            get => (Buff_Holding0000.GetPoint(33) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 1) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit2
        {
            get => (Buff_Holding0000.GetPoint(33) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 2) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit3
        {
            get => (Buff_Holding0000.GetPoint(33) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 3) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit4
        {
            get => (Buff_Holding0000.GetPoint(33) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 4) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit5
        {
            get => (Buff_Holding0000.GetPoint(33) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 5) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit6
        {
            get => (Buff_Holding0000.GetPoint(33) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 6) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit7
        {
            get => (Buff_Holding0000.GetPoint(33) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 7) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit8
        {
            get => (Buff_Holding0000.GetPoint(33) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 8) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit9
        {
            get => (Buff_Holding0000.GetPoint(33) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 9) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit10
        {
            get => (Buff_Holding0000.GetPoint(33) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 10) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit11
        {
            get => (Buff_Holding0000.GetPoint(33) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 11) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit12
        {
            get => (Buff_Holding0000.GetPoint(33) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 12) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit13
        {
            get => (Buff_Holding0000.GetPoint(33) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 13) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit14
        {
            get => (Buff_Holding0000.GetPoint(33) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 14) }, 0, Buff_Holding0000, 33, 1);
            }
        }

        public bool Buff_33_Bit15
        {
            get => (Buff_Holding0000.GetPoint(33) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[33].SetBit(value, 15) }, 0, Buff_Holding0000, 33, 1);
            }
        }
        #endregion

        #region 34
        public bool Buff_34_Bit0
        {
            get => (Buff_Holding0000.GetPoint(34) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 0) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit1
        {
            get => (Buff_Holding0000.GetPoint(34) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 1) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit2
        {
            get => (Buff_Holding0000.GetPoint(34) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 2) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit3
        {
            get => (Buff_Holding0000.GetPoint(34) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 3) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit4
        {
            get => (Buff_Holding0000.GetPoint(34) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 4) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit5
        {
            get => (Buff_Holding0000.GetPoint(34) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 5) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit6
        {
            get => (Buff_Holding0000.GetPoint(34) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 6) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit7
        {
            get => (Buff_Holding0000.GetPoint(34) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 7) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit8
        {
            get => (Buff_Holding0000.GetPoint(34) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 8) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit9
        {
            get => (Buff_Holding0000.GetPoint(34) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 9) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit10
        {
            get => (Buff_Holding0000.GetPoint(34) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 10) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit11
        {
            get => (Buff_Holding0000.GetPoint(34) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 11) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit12
        {
            get => (Buff_Holding0000.GetPoint(34) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 12) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit13
        {
            get => (Buff_Holding0000.GetPoint(34) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 13) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit14
        {
            get => (Buff_Holding0000.GetPoint(34) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 14) }, 0, Buff_Holding0000, 34, 1);
            }
        }

        public bool Buff_34_Bit15
        {
            get => (Buff_Holding0000.GetPoint(34) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[34].SetBit(value, 15) }, 0, Buff_Holding0000, 34, 1);
            }
        }
        #endregion

        #region 35
        public bool Buff_35_Bit0
        {
            get => (Buff_Holding0000.GetPoint(35) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 0) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit1
        {
            get => (Buff_Holding0000.GetPoint(35) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 1) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit2
        {
            get => (Buff_Holding0000.GetPoint(35) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 2) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit3
        {
            get => (Buff_Holding0000.GetPoint(35) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 3) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit4
        {
            get => (Buff_Holding0000.GetPoint(35) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 4) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit5
        {
            get => (Buff_Holding0000.GetPoint(35) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 5) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit6
        {
            get => (Buff_Holding0000.GetPoint(35) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 6) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit7
        {
            get => (Buff_Holding0000.GetPoint(35) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 7) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit8
        {
            get => (Buff_Holding0000.GetPoint(35) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 8) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit9
        {
            get => (Buff_Holding0000.GetPoint(35) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 9) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit10
        {
            get => (Buff_Holding0000.GetPoint(35) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 10) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit11
        {
            get => (Buff_Holding0000.GetPoint(35) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 11) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit12
        {
            get => (Buff_Holding0000.GetPoint(35) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 12) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit13
        {
            get => (Buff_Holding0000.GetPoint(35) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 13) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit14
        {
            get => (Buff_Holding0000.GetPoint(35) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 14) }, 0, Buff_Holding0000, 35, 1);
            }
        }

        public bool Buff_35_Bit15
        {
            get => (Buff_Holding0000.GetPoint(35) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[35].SetBit(value, 15) }, 0, Buff_Holding0000, 35, 1);
            }
        }
        #endregion

        #region 36
        public bool Buff_36_Bit0
        {
            get => (Buff_Holding0000.GetPoint(36) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 0) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit1
        {
            get => (Buff_Holding0000.GetPoint(36) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 1) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit2
        {
            get => (Buff_Holding0000.GetPoint(36) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 2) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit3
        {
            get => (Buff_Holding0000.GetPoint(36) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 3) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit4
        {
            get => (Buff_Holding0000.GetPoint(36) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 4) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit5
        {
            get => (Buff_Holding0000.GetPoint(36) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 5) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit6
        {
            get => (Buff_Holding0000.GetPoint(36) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 6) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit7
        {
            get => (Buff_Holding0000.GetPoint(36) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 7) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit8
        {
            get => (Buff_Holding0000.GetPoint(36) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 8) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit9
        {
            get => (Buff_Holding0000.GetPoint(36) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 9) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit10
        {
            get => (Buff_Holding0000.GetPoint(36) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 10) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit11
        {
            get => (Buff_Holding0000.GetPoint(36) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 11) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit12
        {
            get => (Buff_Holding0000.GetPoint(36) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 12) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit13
        {
            get => (Buff_Holding0000.GetPoint(36) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 13) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit14
        {
            get => (Buff_Holding0000.GetPoint(36) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 14) }, 0, Buff_Holding0000, 36, 1);
            }
        }

        public bool Buff_36_Bit15
        {
            get => (Buff_Holding0000.GetPoint(36) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[36].SetBit(value, 15) }, 0, Buff_Holding0000, 36, 1);
            }
        }
        #endregion

        #region 37
        public bool Buff_37_Bit0
        {
            get => (Buff_Holding0000.GetPoint(37) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 0) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit1
        {
            get => (Buff_Holding0000.GetPoint(37) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 1) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit2
        {
            get => (Buff_Holding0000.GetPoint(37) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 2) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit3
        {
            get => (Buff_Holding0000.GetPoint(37) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 3) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit4
        {
            get => (Buff_Holding0000.GetPoint(37) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 4) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit5
        {
            get => (Buff_Holding0000.GetPoint(37) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 5) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit6
        {
            get => (Buff_Holding0000.GetPoint(37) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 6) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit7
        {
            get => (Buff_Holding0000.GetPoint(37) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 7) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit8
        {
            get => (Buff_Holding0000.GetPoint(37) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 8) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit9
        {
            get => (Buff_Holding0000.GetPoint(37) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 9) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit10
        {
            get => (Buff_Holding0000.GetPoint(37) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 10) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit11
        {
            get => (Buff_Holding0000.GetPoint(37) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 11) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit12
        {
            get => (Buff_Holding0000.GetPoint(37) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 12) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit13
        {
            get => (Buff_Holding0000.GetPoint(37) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 13) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit14
        {
            get => (Buff_Holding0000.GetPoint(37) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 14) }, 0, Buff_Holding0000, 37, 1);
            }
        }

        public bool Buff_37_Bit15
        {
            get => (Buff_Holding0000.GetPoint(37) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[37].SetBit(value, 15) }, 0, Buff_Holding0000, 37, 1);
            }
        }
        #endregion

        #region 38
        public bool Buff_38_Bit0
        {
            get => (Buff_Holding0000.GetPoint(38) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 0) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit1
        {
            get => (Buff_Holding0000.GetPoint(38) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 1) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit2
        {
            get => (Buff_Holding0000.GetPoint(38) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 2) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit3
        {
            get => (Buff_Holding0000.GetPoint(38) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 3) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit4
        {
            get => (Buff_Holding0000.GetPoint(38) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 4) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit5
        {
            get => (Buff_Holding0000.GetPoint(38) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 5) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit6
        {
            get => (Buff_Holding0000.GetPoint(38) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 6) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit7
        {
            get => (Buff_Holding0000.GetPoint(38) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 7) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit8
        {
            get => (Buff_Holding0000.GetPoint(38) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 8) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit9
        {
            get => (Buff_Holding0000.GetPoint(38) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 9) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit10
        {
            get => (Buff_Holding0000.GetPoint(38) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 10) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit11
        {
            get => (Buff_Holding0000.GetPoint(38) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 11) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit12
        {
            get => (Buff_Holding0000.GetPoint(38) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 12) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit13
        {
            get => (Buff_Holding0000.GetPoint(38) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 13) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit14
        {
            get => (Buff_Holding0000.GetPoint(38) & 16384) == 16384;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 14) }, 0, Buff_Holding0000, 38, 1);
            }
        }

        public bool Buff_38_Bit15
        {
            get => (Buff_Holding0000.GetPoint(38) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[38].SetBit(value, 15) }, 0, Buff_Holding0000, 38, 1);
            }
        }
        #endregion

        #region 39
        public bool Buff_39_Bit0
        {
            get => (Buff_Holding0000.GetPoint(39) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 0) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit1
        {
            get => (Buff_Holding0000.GetPoint(39) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 1) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit2
        {
            get => (Buff_Holding0000.GetPoint(39) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 2) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit3
        {
            get => (Buff_Holding0000.GetPoint(39) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 3) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit4
        {
            get => (Buff_Holding0000.GetPoint(39) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 4) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit5
        {
            get => (Buff_Holding0000.GetPoint(39) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 5) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit6
        {
            get => (Buff_Holding0000.GetPoint(39) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 6) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit7
        {
            get => (Buff_Holding0000.GetPoint(39) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 7) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit8
        {
            get => (Buff_Holding0000.GetPoint(39) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 8) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit9
        {
            get => (Buff_Holding0000.GetPoint(39) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 9) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit10
        {
            get => (Buff_Holding0000.GetPoint(39) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 10) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit11
        {
            get => (Buff_Holding0000.GetPoint(39) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 11) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit12
        {
            get => (Buff_Holding0000.GetPoint(39) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 12) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit13
        {
            get => (Buff_Holding0000.GetPoint(39) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 13) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit14
        {
            get => (Buff_Holding0000.GetPoint(39) & 16394) == 16394;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 14) }, 0, Buff_Holding0000, 39, 1);
            }
        }

        public bool Buff_39_Bit15
        {
            get => (Buff_Holding0000.GetPoint(39) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[39].SetBit(value, 15) }, 0, Buff_Holding0000, 39, 1);
            }
        }
        #endregion

        #region 40
        public bool Buff_40_Bit0
        {
            get => (Buff_Holding0000.GetPoint(40) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 0) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit1
        {
            get => (Buff_Holding0000.GetPoint(40) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 1) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit2
        {
            get => (Buff_Holding0000.GetPoint(40) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 2) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit3
        {
            get => (Buff_Holding0000.GetPoint(40) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 3) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit4
        {
            get => (Buff_Holding0000.GetPoint(40) & 16) == 16;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 4) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit5
        {
            get => (Buff_Holding0000.GetPoint(40) & 32) == 32;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 5) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit6
        {
            get => (Buff_Holding0000.GetPoint(40) & 64) == 64;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 6) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit7
        {
            get => (Buff_Holding0000.GetPoint(40) & 128) == 128;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 7) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit8
        {
            get => (Buff_Holding0000.GetPoint(40) & 256) == 256;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 8) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit9
        {
            get => (Buff_Holding0000.GetPoint(40) & 512) == 512;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 9) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit10
        {
            get => (Buff_Holding0000.GetPoint(40) & 1024) == 1024;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 10) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit11
        {
            get => (Buff_Holding0000.GetPoint(40) & 2048) == 2048;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 11) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit12
        {
            get => (Buff_Holding0000.GetPoint(40) & 4096) == 4096;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 12) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit13
        {
            get => (Buff_Holding0000.GetPoint(40) & 8192) == 8192;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 13) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit14
        {
            get => (Buff_Holding0000.GetPoint(40) & 16404) == 16404;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 14) }, 0, Buff_Holding0000, 40, 1);
            }
        }

        public bool Buff_40_Bit15
        {
            get => (Buff_Holding0000.GetPoint(40) >> 15) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[40].SetBit(value, 15) }, 0, Buff_Holding0000, 40, 1);
            }
        }
        #endregion

        public ushort YearOfTime
        {
            get => Buff_Holding0000.GetPoint(41);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 41, 1);
            }
        }

        public ushort MonthOfTime
        {
            get => Buff_Holding0000.GetPoint(42);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 42, 1);
            }
        }

        public ushort DayOfTime
        {
            get => Buff_Holding0000.GetPoint(43);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 43, 1);
            }
        }

        public ushort HourOfTime
        {
            get => Buff_Holding0000.GetPoint(44);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 44, 1);
            }
        }

        public ushort MinuteOfTime
        {
            get => Buff_Holding0000.GetPoint(45);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 45, 1);
            }
        }

        public ushort SecondOfTime
        {
            get => Buff_Holding0000.GetPoint(46);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 46, 1);
            }
        }

        public ushort WeekDayOfTime
        {
            get => Buff_Holding0000.GetPoint(47);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 47, 1);
            }
        }

        public ushort LcdLanguage
        {
            get
            {
                return Buff_Holding0000.GetPoint(48);
            }
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 48, 1);
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComboxItemViewModel> _lcdLanguages;
        private void LcdLanguagesInitialize()
        {
            LcdLanguages = [..
                new[] {
                    new ComboxItemViewModel { Value = "0", Display = MultiLang.GetString("简体中文") },
                    new ComboxItemViewModel { Value = "1", Display = MultiLang.GetString("英语") },
                    new ComboxItemViewModel { Value = "2", Display = MultiLang.GetString("德语") },
                    new ComboxItemViewModel { Value = "3", Display = MultiLang.GetString("西班牙语") },
                    new ComboxItemViewModel { Value = "4", Display = MultiLang.GetString("法语") },
                    new ComboxItemViewModel { Value = "5", Display = MultiLang.GetString("意大利语") },
                    new ComboxItemViewModel { Value = "6", Display = MultiLang.GetString("波兰语") },
                    new ComboxItemViewModel { Value = "7", Display = MultiLang.GetString("意大利语") },
                    new ComboxItemViewModel { Value = "8", Display = MultiLang.GetString("匈牙利语") }
                }];
        }

        public ushort HmiBaudRate
        {
            get
            {
                return Buff_Holding0000.GetPoint(49);
            }
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 49, 1);
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComboxItemViewModel> _hmiBaudRates;
        private void HmiBaudRatesInitialize()
        {
            HmiBaudRates = [..
                new[] {
                    new ComboxItemViewModel { Value = "0", Display = "9600" },
                    new ComboxItemViewModel { Value = "1", Display = "38400" },
                    new ComboxItemViewModel { Value = "2", Display ="115200"}
                }];
        }

        public ushort HmiAddress
        {
            get => Buff_Holding0000.GetPoint(50);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 50, 1);
            }
        }

        public ushort UpgradeFirmwareAddress
        {
            get => Buff_Holding0000.GetPoint(51);
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 51, 1);
            }
        }
        public bool Buff_52_Bit0
        {
            get => (Buff_Holding0000.GetPoint(52) & 1) == 1;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[52].SetBit(value, 0) }, 0, Buff_Holding0000, 52, 1);
            }
        }

        public bool Buff_52_Bit1
        {
            get => (Buff_Holding0000.GetPoint(52) & 2) == 2;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[52].SetBit(value, 1) }, 0, Buff_Holding0000, 52, 1);
            }
        }

        public bool Buff_52_Bit2
        {
            get => (Buff_Holding0000.GetPoint(52) & 4) == 4;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[52].SetBit(value, 2) }, 0, Buff_Holding0000, 52, 1);
            }
        }

        public bool Buff_52_Bit3
        {
            get => (Buff_Holding0000.GetPoint(52) & 8) == 8;
            set
            {
                Array.Copy(new[] { Buff_Holding0000[52].SetBit(value, 3) }, 0, Buff_Holding0000, 52, 1);
            }
        }

        public ushort Debugabled
        {
            get
            {
                return Buff_Holding0000.GetPoint(53);
            }
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 53, 1);
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComboxItemViewModel> _debugableds;
        private void DebugabledsInitialize()
        {
            Debugableds = [..
                new[] {
                    new ComboxItemViewModel { Value = "0", Display = MultiLang.GetString("失能") },
                    new ComboxItemViewModel { Value = "1", Display = MultiLang.GetString("使能") }
                }];
        }

        public ushort DebugProtectable
        {
            get
            {
                return Buff_Holding0000.GetPoint(54);
            }
            set
            {
                Array.Copy(new[] { value }, 0, Buff_Holding0000, 54, 1);
            }
        }

        [ObservableProperty]
        private ObservableCollection<ComboxItemViewModel> _debugProtectables;
        private void DebugProtectablesInitialize()
        {
            DebugProtectables = [..
                new[] {
                    new ComboxItemViewModel { Value = "0", Display = MultiLang.GetString("失能") },
                    new ComboxItemViewModel { Value = "1", Display = MultiLang.GetString("使能") }
                }];
        }

        [ObservableProperty]
        private bool _canWriteInfo;

        [ObservableProperty]
        private string _sn;

        [ObservableProperty]
        private string _collectSn;

        [ObservableProperty]
        private string _pcsSn;

        [ObservableProperty]
        private string _bootVersion;

        [ObservableProperty]
        private string _emsFirmware;

        [ObservableProperty]
        private string _bmsFirmware;

        [ObservableProperty]
        private string hmiFirmware;

        protected override async Task ResetSystemParam01()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x08,
                    StartingAddress = 0,
                    Count = 125
                });

                await Task.Delay(50);
                await QuerySystemParam01(@lock);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.ResetSystemParam01");
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task QuerySystemParam01(object lockObj)
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            SemaphoreSlim @lock = default;
            if (lockObj is null)
            {
                @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            }

            try
            {
                await @lock?.WaitAsync(-1);

                var buff = await modbus.ReadHoldingRegisters(client.Options.SlaveId, 0, (ushort)55);
                Buff_Holding0000 = buff.ToArray();

                SystemParam01Queried = true;
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.QuerySystemParam01");
            }
            finally
            {
                @lock?.Release();
            }

        }


        protected override async Task WriteSystemParam01()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            SemaphoreSlim @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);

            try
            {
                await @lock?.WaitAsync(-1);

                var buff = await modbus.ReadHoldingRegisters(client.Options.SlaveId, 59, (ushort)65);
                Sn = buff.GetStringFromBigEndian(0, 15).TrimEnd('\0');
                CollectSn = buff.GetStringFromBigEndian(15, 8).TrimEnd('\0');
                PcsSn = buff.GetStringFromBigEndian(23, 8).TrimEnd('\0');
                BootVersion = buff.GetStringFromBigEndian(31, 4).TrimEnd('\0');
                EmsFirmware = buff.GetStringFromBigEndian(35, 10).TrimEnd('\0');
                BmsFirmware = buff.GetStringFromBigEndian(45, 10).TrimEnd('\0');
                HmiFirmware = buff.GetStringFromBigEndian(55, 10).TrimEnd('\0');

                CanWriteInfo = true;
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.QuerySystemParam01");
            }
            finally
            {
                @lock?.Release();
            }

        }

        protected override async Task RebootSystem()
        {

            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x06,
                    StartingAddress = 2,
                    Count = 1
                });

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.RebootSystem");
            }
            finally
            {
                @lock.Release();
            }
        }

        protected override async Task ResetSystemFaults()
        {

            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x06,
                    StartingAddress = 3,
                    Count = 1
                });

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.ResetSystemFaults");
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task FactoryReset01()
        {

            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x06,
                    StartingAddress = 4,
                    Count = 1
                });

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.FactoryReset01");
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task FactoryReset02()
        {

            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x06,
                    StartingAddress = 5,
                    Count = 1
                });

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.FactoryReset02");
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task FactoryReset03()
        {

            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x06,
                    StartingAddress = 6,
                    Count = 1
                });

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.FactoryReset03");
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task ReadInfo()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            SemaphoreSlim @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);

            try
            {
                await @lock?.WaitAsync(-1);

                var buff = await modbus.ReadHoldingRegisters(client.Options.SlaveId, 59, (ushort)65);
                Sn = buff.GetStringFromBigEndian(0, 15).TrimEnd('\0');
                CollectSn = buff.GetStringFromBigEndian(15, 8).TrimEnd('\0');
                PcsSn = buff.GetStringFromBigEndian(23, 8).TrimEnd('\0');
                BootVersion = buff.GetStringFromBigEndian(31, 4).TrimEnd('\0');
                EmsFirmware = buff.GetStringFromBigEndian(35, 10).TrimEnd('\0');
                BmsFirmware = buff.GetStringFromBigEndian(45, 10).TrimEnd('\0');
                HmiFirmware = buff.GetStringFromBigEndian(55, 10).TrimEnd('\0');

                CanWriteInfo = true;
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.ReadInfo");
            }
            finally
            {
                @lock?.Release();
            }

        }
        #endregion

        #region system param02
        protected override async Task ResetSystemParam02()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x08,
                    StartingAddress = 125,
                    Count = 93
                });

                await Task.Delay(50);
                await QuerySystemParam02(@lock);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.ResetSystemParam02");
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task QuerySystemParam02(object lockObj)
        {

        }

        protected override async Task WriteSystemParam02()
        {

        }


        #endregion

        #region 48 period time
        protected override async Task Write48TimeSlot()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var index = PeriodTimers.IndexOf(SelectedTimer);
            if (index <= 0)
            {
                return;
            }

            var address = GetTimeSlot48StartAddress(index);

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = TimeSlots48.Select(s => (ushort)((short)s.Value * 10)).ToArray().GetBytesFromBigEndian();
                await modbus.WriteHoldingRegisters(client.Options.SlaveId, address, buff);

                await Task.Delay(50);
                await Query48TimeSlotData(@lock);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.Reset48TimeSlot");
                UiGlobalContext.EnqueueRootMessage(MultiLang.GetString("写入失败: " + ex.Message));
            }
            finally
            {
                @lock.Release();
            }

        }

        protected override async Task Reset48TimeSlot()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var index = PeriodTimers.IndexOf(SelectedTimer);
            if (index <= 0)
            {
                return;
            }

            var address = GetTimeSlot48StartAddress(index);

            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            try
            {
                await @lock.WaitAsync();


                var buff = await modbus.SendModbusRequestExtAsync(new Models.ModbusRequestExt
                {
                    FunctionCode = 0x08,
                    StartingAddress = address,
                    Count = 48
                });

                await Task.Delay(50);
                await Query48TimeSlotData(@lock);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.Reset48TimeSlot");
            }
            finally
            {
                @lock.Release();
            }
        }

        protected override async Task Query48TimeSlotData(object lockObj)
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || client is not IModbusObject modbus)
            {
                return;
            }

            var index = PeriodTimers.IndexOf(SelectedTimer);
            if (index <= 0)
            {
                return;
            }

            SemaphoreSlim @lock = default;
            if (lockObj is null)
            {
                @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            }

            try
            {
                await @lock?.WaitAsync(-1);

                var address = GetTimeSlot48StartAddress(index);
                var buff = await modbus.ReadHoldingRegisters(client.Options.SlaveId, address, (ushort)TimeSlots48.Count);
                if (buff.Length != TimeSlots48.Count)
                {
                    return;
                }

                index = 0;
                var array = buff.ToArray();
                foreach (var item in TimeSlots48)
                {
                    item.Value = (short)array[index++] * 0.1M;
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "SystemSettingViewModel.Query48TimeSlotData");
            }
            finally
            {
                @lock?.Release();
            }
        }

        private ushort GetTimeSlot48StartAddress(int index)
        {
            return index switch
            {
                1 => 294,
                2 => 2044,
                3 => 2169,
                4 => 2294,
                _ => 294
            };
        }
        #endregion

        private void MultiLang_OnLanguageChanged(string currLang, string nextLang)
        {
            SystemPowerStatusesInitialize();
            LcdLanguagesInitialize();
            DebugabledsInitialize();
            DebugProtectablesInitialize();
        }

        public override void Load()
        {
            var list = CommunicateAdapterPool.ModbusMasterPool.Keys
                .ToList();
            Links = [.. list];
            if (Links?.Any() is true)
            {
                SelectedLink = Links.First();
            }

            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}
