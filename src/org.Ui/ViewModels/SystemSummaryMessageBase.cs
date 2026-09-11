using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Messagings;
using org.Ui.Messagings;
using org.Ui.MultiLanguage;
using org.Utils;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace org.Ui.ViewModels
{
    /// <summary>
    /// 此代码参考product01，若虚函数及属性部分不满足，则在子类重写虚函数及属性，并按需要调用父类的虚函数
    /// </summary>
    public partial class SystemSummaryMessageBase : ObservableObject
    {
        protected const string NC = "NC";
        protected string SelectedLink;
        #region holding 0000
        /// <summary>
        /// 1000: 液冷, 2000: 风冷
        /// </summary>
        public virtual ushort DTC { get; }

        [ObservableProperty]
        private string _dissipationType;


        protected virtual string GetDissipationType(ushort dtc)
        {
            return dtc switch
            {
                1000 => MultiLang.GetString(MultiLang.Instance.LiquidCooling),
                2000 => MultiLang.GetString(MultiLang.Instance.LiquidCooling),
                _ => string.Empty
            };
        }
        #endregion
        #region holding 0125
        [ObservableProperty]
        private ObservableCollection<KeyValuePairViewModel> _bmsOnlines;
        [ObservableProperty]
        private ObservableCollection<KeyValuePairViewModel> _pcsOnlines;
        [ObservableProperty]
        private ObservableCollection<KeyValuePairViewModel> _dcdcOnlines;
        [ObservableProperty]
        private ObservableCollection<KeyValuePairViewModel> _mpptOnlines;
        [ObservableProperty]
        private ObservableCollection<KeyValuePairViewModel> _meterOnlines;

        public virtual ushort ParallelNumber { get; }
        #endregion
        #region holding 1875
        public virtual decimal GeneratingSystemPowerHourly { get; set; }
        public virtual decimal GeneratingSystemPowerDaily { get; set; }
        public virtual decimal GeneratingSystemPowerMonthly { get; set; }
        public virtual decimal GeneratingSystemPowerYearly { get; set; }
        public virtual decimal GeneratingSystemPowerTotal { get; set; }
        public virtual decimal GeneratingSelfPowerHourly { get; set; }
        public virtual decimal GeneratingSelfPowerDaily { get; set; }
        public virtual decimal GeneratingSelfPowerMonthly { get; set; }
        public virtual decimal GeneratingSelfPowerYearly { get; set; }
        public virtual decimal GeneratingSelfPowerTotal { get; set; }
        public virtual decimal GeneratingFeedingPowerHourly { get; set; }
        public virtual decimal GeneratingFeedingPowerDaily { get; set; }
        public virtual decimal GeneratingFeedingPowerMonthly { get; set; }
        public virtual decimal GeneratingFeedingPowerYearly { get; set; }
        public virtual decimal GeneratingFeedingPowerTotal { get; set; }
        public virtual decimal GeneratingFetchingPowerHourly { get; set; }
        public virtual decimal GeneratingFetchingPowerDaily { get; set; }
        public virtual decimal GeneratingFetchingPowerMonthly { get; set; }
        public virtual decimal GeneratingFetchingPowerYearly { get; set; }
        public virtual decimal GeneratingFetchingPowerTotal { get; set; }
        public virtual decimal GeneratingLoadPowerHourly { get; set; }
        public virtual decimal GeneratingLoadPowerDaily { get; set; }
        public virtual decimal GeneratingLoadPowerMonthly { get; set; }
        public virtual decimal GeneratingLoadPowerYearly { get; set; }
        public virtual decimal GeneratingLoadPowerTotal { get; set; }
        public virtual decimal GeneratingPvPowerHourly { get; set; }
        public virtual decimal GeneratingPvPowerDaily { get; set; }
        public virtual decimal GeneratingPvPowerMonthly { get; set; }
        public virtual decimal GeneratingPvPowerYearly { get; set; }
        public virtual decimal GeneratingPvPowerTotal { get; set; }
        public virtual decimal GeneratingBatteryDischargePowerHourly { get; set; }
        public virtual decimal GeneratingBatteryDischargePowerDaily { get; set; }
        public virtual decimal GeneratingBatteryDischargePowerMonthly { get; set; }
        public virtual decimal GeneratingBatteryDischargePowerYearly { get; set; }
        public virtual decimal GeneratingBatteryDischargePowerTotal { get; set; }
        public virtual decimal GeneratingBatteryChargePowerHourly { get; set; }
        public virtual decimal GeneratingBatteryChargePowerDaily { get; set; }
        public virtual decimal GeneratingBatteryChargePowerMonthly { get; set; }
        public virtual decimal GeneratingBatteryChargePowerYearly { get; set; }
        public virtual decimal GeneratingBatteryChargePowerTotal { get; set; }
        public virtual decimal GeneratingPowerDaily { get; set; }
        public virtual decimal GeneratingPowerTotal { get; set; }
        public virtual decimal GeneratingAcDcPowerDaily { get; set; }
        public virtual decimal GeneratingAcDcPowerTotal { get; set; }
        public virtual decimal GeneratingDcAcPowerDaily { get; set; }
        public virtual decimal GeneratingDcAcPowerTotal { get; set; }

        #endregion
        #region input 0000

        public virtual ushort SystemState { get; }
        [ObservableProperty]
        private string _systemStateText;

        protected virtual string GetSystemStateText(ushort systemState)
        {
            return systemState switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.设备状态_初始化),
                1 => MultiLang.GetString(MultiLang.Instance.设备状态_自检),
                2 => MultiLang.GetString(MultiLang.Instance.设备状态_停机),
                3 => MultiLang.GetString(MultiLang.Instance.设备状态_旁路),
                4 => MultiLang.GetString(MultiLang.Instance.设备状态_就绪),
                5 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.设备状态_并网充电),
                5 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_并网运行),
                6 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.设备状态_并网放电),
                6 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_预留),
                7 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.设备状态_离网充电),
                7 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_离网预留),
                8 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.设备状态_离网放电),
                8 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_预留),
                9 => MultiLang.GetString(MultiLang.Instance.设备状态_告警),
                10 => MultiLang.GetString(MultiLang.Instance.设备状态_故障),
                11 => MultiLang.GetString(MultiLang.Instance.设备状态_升级),
                12 => MultiLang.GetString(MultiLang.Instance.设备状态_手动调试),
                13 => MultiLang.GetString(MultiLang.Instance.设备状态_保护性充电),
                _ => string.Empty
            };
        }

        public virtual ushort RunningMode { get; }
        [ObservableProperty]
        private string _runningModeText;
        protected virtual string GetRunningMode(ushort runningMode)
        {
            return runningMode switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.设备状态_并网定时充放模式),
                1 => MultiLang.GetString(MultiLang.Instance.设备状态_并网削峰填谷模式),
                2 => MultiLang.GetString(MultiLang.Instance.设备状态_并网自发自用模式),
                3 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.设备状态_离网模式),
                3 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_并网电池优先模式),
                4 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.设备状态_远程模式),
                4 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_离网模式),
                5 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_远程模式),
                6 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.设备状态_离网油机模式),
                _ => string.Empty
            };
        }

        public virtual bool BmsOnlineState { get; }
        public virtual bool FireOnlineState { get; }
        public virtual bool PcsOnlineState { get; }
        public virtual bool DcdcOnlineState { get; }
        public virtual bool MpptOnlineState { get; }
        public virtual bool StsOnlineState { get; }
        public virtual bool ElectricOnlineState { get; }
        public virtual bool AirConditionOnlineState { get; }
        public virtual bool LiquidOnlineState { get; }
        public virtual bool HmiOnlineState { get; }
        public virtual bool PcOnlineState { get; }
        public virtual bool GpsOnlineState { get; }
        public virtual bool FourGOnlineState { get; }
        public virtual bool WifiOnlineState { get; }
        public virtual bool BluetoothOnlineState { get; }
        public virtual bool Lan1OnlineState { get; }
        public virtual bool Lan2OnlineState { get; }
        public virtual bool HumidityAndTempOnlineState { get; }
        public virtual bool GridConnectedOnlineState { get; }

        public virtual ushort GridConnected { get; }

        [ObservableProperty]
        private string _gridConnectedText;
        protected virtual string GetGridConnectedText(ushort gridConnected)
        {
            return gridConnected switch
            {
                0 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.离网),
                0 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.离网),
                1 when DTC == 2000 => MultiLang.GetString(MultiLang.Instance.并网),
                1 when DTC == 1000 => MultiLang.GetString(MultiLang.Instance.并网),
                _ => MultiLang.GetString(MultiLang.Instance.离网),
            };
        }

        public virtual ushort Onlined { get; }

        [ObservableProperty]
        private string _onlinedText;
        protected virtual string GetOnlineText(ushort onlined)
        {
            return onlined switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.离线),
                1 => MultiLang.GetString(MultiLang.Instance.在线),
                _ => MultiLang.GetString(MultiLang.Instance.离线)
            };
        }

        public virtual ushort EmsState { get; }

        [ObservableProperty]
        private string _emsStateText;
        protected virtual string GetEmsStateText(ushort emsState)
        {
            return emsState switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.等待),
                1 => MultiLang.GetString(MultiLang.Instance.故障),
                2 => MultiLang.GetString(MultiLang.Instance.运行中),
                _ => string.Empty
            };
        }

        public virtual ushort ConfigFileCode { get; }

        public virtual ushort FanRpm { get; }

        [ObservableProperty]
        private string _fanRpmText;
        protected virtual string GetFanRpmText(ushort fanRpm)
        {
            if (DTC == 1000)
            {
                return "NC";
            }

            return fanRpm.ToString();
        }

        public virtual int StateOfCharge { get; }
        [ObservableProperty]
        private string _stateOfChargeText;
        protected virtual string GetStateOfChargeText(int soc)
        {
            return (soc / 1000.00f).ToString();
        }

        public virtual decimal Humidity01 { get; }
        public virtual decimal Humidity02 { get; }

        public virtual decimal Temperature01 { get; }
        public virtual decimal Temperature02 { get; }
        public virtual decimal Temperature03 { get; }
        public virtual decimal Temperature04 { get; }
        public virtual string EmsFirmware { get; }
        public virtual string BmsFirmware { get; }
        public virtual string HmiFirmware { get; }

        [ObservableProperty]
        private ushort _fault01;
        [ObservableProperty]
        private ushort _fault02;
        [ObservableProperty]
        private ushort _fault03;
        [ObservableProperty]
        private ushort _fault04;
        [ObservableProperty]
        private ushort _fault05;
        [ObservableProperty]
        private ushort _warning01;
        [ObservableProperty]
        private ushort _warning02;
        [ObservableProperty]
        private ushort _warning03;
        [ObservableProperty]
        private ushort _warning04;
        [ObservableProperty]
        private ushort _warning05;

        public virtual int GridPower { get; }
        public virtual decimal GridFrequency { get; }
        public virtual decimal GridAVoltage { get; }
        public virtual decimal GridACurrent { get; }
        public virtual decimal GridAPower { get; }
        public virtual decimal GridBVoltage { get; }
        public virtual decimal GridBCurrent { get; }
        public virtual decimal GridBPower { get; }
        public virtual decimal GridCVoltage { get; }
        public virtual decimal GridCCurrent { get; }
        public virtual decimal GridCPower { get; }
        public virtual decimal GridLineAVoltage { get; }
        public virtual decimal GridLineBVoltage { get; }
        public virtual decimal GridLineCVoltage { get; }
        public virtual decimal GridAPowerFactor { get; }
        public virtual decimal GridBPowerFactor { get; }
        public virtual decimal GridCPowerFactor { get; }
        public virtual decimal GridTotalPowerFactor { get; }
        public virtual ushort GridState { get; }
        [ObservableProperty]
        private string _gridStateText;
        public virtual string GetGridStateText(ushort gridState)
        {
            return gridState switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.待机),
                1 => MultiLang.GetString(MultiLang.Instance.设备状态_取电),
                2 => MultiLang.GetString(MultiLang.Instance.设备状态_馈电),
                _ => string.Empty
            };
        }

        public virtual uint PvInputPower { get; }
        public virtual ushort PvMpptCount { get; }
        public virtual string Pv1VoltageText { get; }
        public virtual string Pv1CurrentText { get; }
        public virtual string Pv2VoltageText { get; }
        public virtual string Pv2CurrentText { get; }
        public virtual string Pv3VoltageText { get; }
        public virtual string Pv3CurrentText { get; }
        public virtual string Pv4VoltageText { get; }
        public virtual string Pv4CurrentText { get; }
        public virtual string Pv5VoltageText { get; }
        public virtual string Pv5CurrentText { get; }
        public virtual string Pv6VoltageText { get; }
        public virtual string Pv6CurrentText { get; }
        public virtual string Pv7VoltageText { get; }
        public virtual string Pv7CurrentText { get; }
        public virtual string Pv8VoltageText { get; }
        public virtual string Pv8CurrentText { get; }
        #endregion
        #region input 0125

        [ObservableProperty]
        private decimal _inverFrequency;
        [ObservableProperty]
        private int _inverTotalPower;
        [ObservableProperty]
        private decimal _inverAVoltage;
        [ObservableProperty]
        private decimal _inverACurrent;
        [ObservableProperty]
        private decimal _inverAPower;
        [ObservableProperty]
        private decimal _inverBVoltage;
        [ObservableProperty]
        private decimal _inverBCurrent;
        [ObservableProperty]
        private decimal _inverBPower;
        [ObservableProperty]
        private decimal _inverCVoltage;
        [ObservableProperty]
        private decimal _inverCCurrent;
        [ObservableProperty]
        private decimal _inverCPower;
        [ObservableProperty]
        private decimal _inverPowerFactor;

        public virtual ushort InverState { get; }
        [ObservableProperty]
        private string _inverStateText;

        public virtual decimal LoadAVoltage { get; }
        public virtual decimal LoadACurrent { get; }
        public virtual uint LoadAPower { get; }
        public virtual decimal LoadAPowerFactor { get; }
        public virtual decimal LoadBVoltage { get; }
        public virtual decimal LoadBCurrent { get; }
        public virtual uint LoadBPower { get; }
        public virtual decimal LoadBPowerFactor { get; }
        public virtual decimal LoadCVoltage { get; }
        public virtual decimal LoadCCurrent { get; }
        public virtual uint LoadCPower { get; }
        public virtual decimal LoadCPowerFactor { get; }
        public virtual uint LoadTotalPower { get; }
        public virtual decimal LoadPowerFactor { get; }
        public virtual decimal LoadFrequency { get; }

        public virtual int BatteryPower { get; }
        public virtual decimal BatteryVoltage { get; }
        public virtual ushort BatterySoc { get; }
        public virtual decimal BatteryCurrent { get; }
        public virtual ushort BatteryState { get; }
        [ObservableProperty]
        private string _batteryStateText;
        public virtual string GetBatteryStateText(ushort batteryState)
        {
            return batteryState switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.待机),
                1 => MultiLang.GetString(MultiLang.Instance.AC_DC),
                2 => MultiLang.GetString(MultiLang.Instance.DC_AC),
                _ => string.Empty
            };
        }
        public virtual decimal GridChargePower { get; }

        public virtual uint GeneratorPower { get; }
        public virtual string GeneratorPowerText { get; }

        public virtual uint SystemEletricPower { get; }
        public virtual string SystemEletricPowerText { get; }
        public virtual uint SelfUsePower { get; }
        public virtual string SelfUsePowerText { get; }
        public virtual uint SolarInverterPower { get; }
        public virtual string SolarInverterPowerText { get; }

        public virtual ushort SystemTimeYear { get; }
        public virtual ushort SystemTimeMonth { get; }
        public virtual ushort SystemTimeDayOfMonth { get; }
        public virtual ushort SystemTimeHour { get; }
        public virtual ushort SystemTimeMinute { get; }
        public virtual ushort SystemTimeSecond { get; }
        public virtual ushort SystemTimeDayOfWeek { get; }


        #endregion

        #region input 250
        public virtual ushort ParallelState { get; }
        [ObservableProperty]
        private string _parallelStateText;
        public string GetParallelStateText(ushort state)
        {
            return state switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.否),
                1 => MultiLang.GetString(MultiLang.Instance.并机),
                _ => string.Empty
            };
        }

        public virtual ushort ParallelIdentity { get; }
        [ObservableProperty]
        private string _parallelIdentityText;
        public string GetParallelIdentityText(ushort identity)
        {
            return identity switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.无效),
                1 => MultiLang.GetString(MultiLang.Instance.主机),
                2 => MultiLang.GetString(MultiLang.Instance.从机),
                _ => string.Empty
            };
        }

        public virtual ushort ParallelId { get; }
        public virtual ushort ParallelCount { get; }
        public virtual string ParallelInvPvPowerText { get; }
        public virtual ushort ParallelMpptCount { get; }
        public virtual int ParallelGridPower { get; }
        public virtual decimal ParallelGridFrequency { get; }
        public virtual ushort ParallelGridState { get; }
        [ObservableProperty]
        private string _parallelGridStateText;
        public virtual string GetParallelGridStateText(ushort state)
        {
            return state switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.待机),
                1 => MultiLang.GetString(MultiLang.Instance.设备状态_取电),
                2 => MultiLang.GetString(MultiLang.Instance.设备状态_馈电),
                _ => string.Empty
            };
        }
        public virtual decimal ParallelTotalPowerFactor { get; }
        public virtual decimal ParallelLineAVoltage { get; }
        public virtual decimal ParallelLineBVoltage { get; }
        public virtual decimal ParallelLineCVoltage { get; }
        public virtual decimal ParallelAVoltage { get; }
        public virtual decimal ParallelACurrent { get; }
        public virtual decimal ParallelAPower { get; }
        public virtual decimal ParallelAPowerFactor { get; }
        public virtual decimal ParallelBVoltage { get; }
        public virtual decimal ParallelBCurrent { get; }
        public virtual decimal ParallelBPower { get; }
        public virtual decimal ParallelBPowerFactor { get; }
        public virtual decimal ParallelCVoltage { get; }
        public virtual decimal ParallelCCurrent { get; }
        public virtual decimal ParallelCPower { get; }
        public virtual decimal ParallelCPowerFactor { get; }
        public virtual string ParallelCode { get; }
        #endregion
        #region input 1875 尖峰平谷（Peak、Sharp、Flat、Vally）
        public virtual decimal PsfvGroupValidTotalPower { get; }
        public virtual decimal PsfvGroupValidPeakPower { get; }
        public virtual decimal PsfvGroupValidSharpPower { get; }
        public virtual decimal PsfvGroupValidFlatPower { get; }
        public virtual decimal PsfvGroupValidVallyPower { get; }

        public virtual decimal PsfvPositiveValidTotalPower { get; }
        public virtual decimal PsfvPositiveValidPeakPower { get; }
        public virtual decimal PsfvPositiveValidSharpPower { get; }
        public virtual decimal PsfvPositiveValidFlatPower { get; }
        public virtual decimal PsfvPositiveValidVallyPower { get; }

        public virtual decimal PsfvOppositeValidTotalPower { get; }
        public virtual decimal PsfvOppositeValidPeakPower { get; }
        public virtual decimal PsfvOppositeValidSharpPower { get; }
        public virtual decimal PsfvOppositeValidFlatPower { get; }
        public virtual decimal PsfvOppositeValidVallyPower { get; }

        public virtual decimal PsfvGroupInvalidTotalPower { get; }
        public virtual decimal PsfvGroupInvalidPeakPower { get; }
        public virtual decimal PsfvGroupInvalidSharpPower { get; }
        public virtual decimal PsfvGroupInvalidFlatPower { get; }
        public virtual decimal PsfvGroupInvalidVallyPower { get; }

        public virtual decimal PsfvPositiveInvalidTotalPower { get; }
        public virtual decimal PsfvPositiveInvalidPeakPower { get; }
        public virtual decimal PsfvPositiveInvalidSharpPower { get; }
        public virtual decimal PsfvPositiveInvalidFlatPower { get; }
        public virtual decimal PsfvPositiveInvalidVallyPower { get; }

        public virtual decimal PsfvOppositeInvalidTotalPower { get; }
        public virtual decimal PsfvOppositeInvalidPeakPower { get; }
        public virtual decimal PsfvOppositeInvalidSharpPower { get; }
        public virtual decimal PsfvOppositeInvalidFlatPower { get; }
        public virtual decimal PsfvOppositeInvalidVallyPower { get; }

        public virtual decimal PsfvPeakUnitPrice { get; }
        public virtual decimal PsfvSharpUnitPrice { get; }
        public virtual decimal PsfvFlatUnitPrice { get; }
        public virtual decimal PsfvVallyUnitPrice { get; }

        #endregion
        #region input 2000


        #endregion
        #region input 2125
        public virtual string HardwareVersion_4G { get; }
        public virtual string SoftwareVersion1_4G { get; }
        public virtual string SoftwareVersion2_4G { get; }
        public virtual string Imei_4G { get; }
        public virtual string Sim_4G { get; }

        #endregion

        public SystemSummaryMessageBase()
        {
            OnlinedText = GetOnlineText(0);
            GridConnectedText = GetGridConnectedText(0);
            BmsOnlines = [];
            PcsOnlines = [];
            DcdcOnlines = [];
            MpptOnlines = [];
            MeterOnlines = [];
            InverStateText = GetInverStateText(0);

            GridStateText = GetGridStateText(0);
            BatteryStateText = GetBatteryStateText(0);
            ParallelStateText = GetParallelStateText(0);
            ParallelIdentityText = GetParallelIdentityText(0);
            ParallelGridStateText = GetParallelGridStateText(0);
        }

        public void OnReceivedMessage(UshortMessage message)
        {
            ReceivingMessage(message);
        }

        protected virtual void ReceivingMessage(UshortMessage message)
        {
            switch (message.Mapping)
            {
                case Input00Message.KEY:
                    Input0000Refreshing(message);
                    break;
                case Input125Message.KEY:
                    Input0125Refreshing(message);
                    break;
                case Input0250Message.KEY:
                    Input0250Refreshing(message);
                    break;
                case Input1875Message.KEY:
                    Input1875Refreshing(message);
                    break;
                case Input2000Message.KEY:
                    Input2000Refreshing(message);
                    break;
                case Input2125Message.KEY:
                    Input2125Refreshing(message);
                    break;
                case Holding00Message.KEY:
                    Holding0000Refreshing(message);
                    break;
                case Holding0125Message.KEY:
                    Holding0125Refreshing(message);
                    break;
                case Holding1875Message.KEY:
                    Holding1875Refreshing(message);
                    break;
            }
        }

        public virtual void Load() { }

        public virtual void UnLoad()
        {

        }

        protected virtual void Holding0000Refreshing(UshortMessage message)
        {
            var newDTC = message.Buffer.GetPoint(0);
            if (DTC != newDTC)
            {
                DissipationType = GetDissipationType(newDTC);
            }
        }

        protected virtual void Holding0125Refreshing(UshortMessage message)
        {
            var newParallelNumber = message.Buffer.GetPoint(208 - 125);
            if (ParallelNumber != newParallelNumber
                || newParallelNumber != BmsOnlines?.Count
                || newParallelNumber != PcsOnlines?.Count
                || newParallelNumber != DcdcOnlines?.Count
                || newParallelNumber != MpptOnlines?.Count
                || newParallelNumber != MeterOnlines?.Count)
            {
                BmsOnlines = [.. Enumerable.Range(0, newParallelNumber)
                    .Select(s => new KeyValuePairViewModel(s.ToString().PadLeft(2, '0'), s.ToString(), false))];
                PcsOnlines = [.. Enumerable.Range(0, newParallelNumber)
                    .Select(s => new KeyValuePairViewModel(s.ToString().PadLeft(2, '0'), s.ToString(), false))];
                DcdcOnlines = [.. Enumerable.Range(0, newParallelNumber)
                    .Select(s => new KeyValuePairViewModel(s.ToString().PadLeft(2, '0'), s.ToString(), false))];
                MpptOnlines = [.. Enumerable.Range(0, newParallelNumber)
                    .Select(s => new KeyValuePairViewModel(s.ToString().PadLeft(2, '0'), s.ToString(), false))];
                MeterOnlines = [.. Enumerable.Range(0, newParallelNumber)
                    .Select(s => new KeyValuePairViewModel(s.ToString().PadLeft(2, '0'), s.ToString(), false))];
            }
        }

        protected virtual void Holding1875Refreshing(UshortMessage message)
        {

        }

        protected virtual void Input2125Refreshing(UshortMessage message)
        {

        }

        protected virtual void Input2000Refreshing(UshortMessage message)
        {

        }

        protected virtual void Input1875Refreshing(UshortMessage message)
        {

        }

        protected virtual void Input0000Refreshing(UshortMessage message)
        {
            var newSystemState = message.Buffer.GetPoint(0);
            if (SystemState != newSystemState)
            {
                SystemStateText = GetSystemStateText(newSystemState);
            }

            var newRunningMode = message.Buffer.GetPoint(1);
            if (RunningMode != newRunningMode)
            {
                RunningModeText = GetRunningMode(newRunningMode);
            }

            var newGridConnected = message.Buffer.GetPoint(13);
            if (GridConnected != newGridConnected)
            {
                GridConnectedText = GetGridConnectedText(newGridConnected);
            }

            var newOnlined = message.Buffer.GetPoint(4);
            if (Onlined != newOnlined)
            {
                OnlinedText = GetOnlineText(newOnlined);
            }

            var newEmsState = message.Buffer.GetPoint(14);
            if (EmsState != newEmsState)
            {
                EmsStateText = GetEmsStateText(newEmsState);
            }

            FanRpmText = GetFanRpmText(message.Buffer.GetPoint(15));

            var newSoc = message.Buffer.GetPoint(10) << 16 | message.Buffer.GetPoint(11);
            StateOfChargeText = GetStateOfChargeText(newSoc);

            var bmsOnline = message.Buffer.GetPoint(5);
            foreach (var item in BmsOnlines)
            {
                item.IsChecked = (bmsOnline >> int.Parse(item.Value) & 1) == 1;
            }
            var pcsOnline = message.Buffer.GetPoint(6);
            foreach (var item in PcsOnlines)
            {
                item.IsChecked = (pcsOnline >> int.Parse(item.Value) & 1) == 1;
            }
            var dcdcOnline = message.Buffer.GetPoint(7);
            foreach (var item in DcdcOnlines)
            {
                item.IsChecked = (dcdcOnline >> int.Parse(item.Value) & 1) == 1;
            }
            var mpptOnline = message.Buffer.GetPoint(8);
            foreach (var item in MpptOnlines)
            {
                item.IsChecked = (mpptOnline >> int.Parse(item.Value) & 1) == 1;
            }
            var meterOnline = message.Buffer.GetPoint(9);
            foreach (var item in MeterOnlines)
            {
                item.IsChecked = (meterOnline >> int.Parse(item.Value) & 1) == 1;
            }

            Fault01 = message.Buffer.GetPoint(23);
            Fault02 = message.Buffer.GetPoint(24);
            Fault03 = message.Buffer.GetPoint(25);
            Fault04 = message.Buffer.GetPoint(26);
            Fault05 = message.Buffer.GetPoint(27);

            Warning01 = message.Buffer.GetPoint(28);
            Warning02 = message.Buffer.GetPoint(29);
            Warning03 = message.Buffer.GetPoint(30);
            Warning04 = message.Buffer.GetPoint(31);
            Warning05 = message.Buffer.GetPoint(32);

            var newGridState = message.Buffer.GetPoint(85);
            if (GridState != newGridState)
            {
                GridStateText = GetGridStateText(newGridState);
            }
        }

        protected virtual void Input0125Refreshing(UshortMessage message)
        {
            InverFrequency = message.Buffer.GetPoint(168 - 125) / 100.0M;
            InverTotalPower = message.Buffer.GetPoint32(169 - 125);

            InverAVoltage = message.Buffer.GetPoint(171 - 125) / 10.0M;
            InverACurrent = (short)message.Buffer.GetPoint(172 - 125) / 100.0M;
            InverAPower = message.Buffer.GetPoint32(173 - 125);

            InverBVoltage = message.Buffer.GetPoint(175 - 125) / 10.0M;
            InverBCurrent = (short)message.Buffer.GetPoint(176 - 125) / 100.0M;
            InverBPower = message.Buffer.GetPoint32(177 - 125);

            InverCVoltage = message.Buffer.GetPoint(179 - 125) / 10.0M;
            InverCCurrent = (short)message.Buffer.GetPoint(180 - 125) / 100.0M;
            InverCPower = message.Buffer.GetPoint32(181 - 125);

            InverPowerFactor = (short)message.Buffer.GetPoint(183) / 100.0M;
            var newInverState = message.Buffer.GetPoint(184 - 125);
            if (InverState != newInverState)
            {
                InverStateText = GetInverStateText(newInverState);
            }

            var newBatteryState = message.Buffer.GetPoint(149 - 125);
            if (BatteryState != newBatteryState)
            {
                BatteryStateText = GetBatteryStateText(newBatteryState);
            }
        }

        protected virtual string GetInverStateText(ushort inverState)
        {
            return inverState switch
            {
                0 => MultiLang.GetString(MultiLang.Instance.待机),
                1 => MultiLang.GetString(MultiLang.Instance.AC_DC),
                2 => MultiLang.GetString(MultiLang.Instance.DC_AC),
                _ => string.Empty
            };
        }

        protected virtual void Input0250Refreshing(UshortMessage message)
        {
            var newParallelState = message.Buffer.GetPoint(250 - 250);
            if (ParallelState != newParallelState)
            {
                ParallelStateText = GetParallelStateText(newParallelState);
            }

            var newParallelIdentity = message.Buffer.GetPoint(251 - 250);
            if (ParallelIdentity != newParallelIdentity)
            {
                ParallelIdentityText = GetParallelIdentityText(newParallelIdentity);
            }

            var newParallelGridState = message.Buffer.GetPoint(288 - 250);
            if (ParallelGridState != newParallelGridState)
            {
                ParallelGridStateText = GetParallelGridStateText(newParallelGridState);
            }
        }

        public virtual void SelectedLinkChanged(string nextLinkKey)
        {
            SelectedLink = nextLinkKey;
        }
    }
}
