using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Communication;
using org.Models.Messagings;
using org.Utils;
using org.Communication.Extensions;
using org.Utils.Global;

namespace org.Product01.ViewModels
{
    public partial class Product01SystemSummaryViewModel
    {
        [ObservableProperty]
        private bool _enableWriteGeneratings;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(GeneratingSystemPowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingSystemPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingSystemPowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingSystemPowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingSystemPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingSelfPowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingSelfPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingSelfPowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingSelfPowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingSelfPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingFeedingPowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingFeedingPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingFeedingPowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingFeedingPowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingFeedingPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingFetchingPowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingFetchingPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingFetchingPowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingFetchingPowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingFetchingPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingLoadPowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingLoadPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingLoadPowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingLoadPowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingLoadPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingPvPowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingPvPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingPvPowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingPvPowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingPvPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryDischargePowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryDischargePowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryDischargePowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryDischargePowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryDischargePowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryChargePowerHourly))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryChargePowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryChargePowerMonthly))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryChargePowerYearly))]
        [NotifyPropertyChangedFor(nameof(GeneratingBatteryChargePowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingAcDcPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingAcDcPowerTotal))]
        [NotifyPropertyChangedFor(nameof(GeneratingDcAcPowerDaily))]
        [NotifyPropertyChangedFor(nameof(GeneratingDcAcPowerTotal))]
        public partial ReadOnlyMemory<ushort> Buff_Holding1875 { get; set; }
        private ushort[] _holding1875;
        public override decimal GeneratingSystemPowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1875 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1875 - 1875) / 10.0M; }
        public override decimal GeneratingSystemPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1877 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1877 - 1875) / 10.0M; }
        public override decimal GeneratingSystemPowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1879 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1879 - 1875) / 10.0M; }
        public override decimal GeneratingSystemPowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1881 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1881 - 1875) / 10.0M; }
        public override decimal GeneratingSystemPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1883 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1883 - 1875) / 10.0M; }

        public override decimal GeneratingSelfPowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1885 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1885 - 1875) / 10.0M; }
        public override decimal GeneratingSelfPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1887 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1887 - 1875) / 10.0M; }
        public override decimal GeneratingSelfPowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1889 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1889 - 1875) / 10.0M; }
        public override decimal GeneratingSelfPowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1891 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1891 - 1875) / 10.0M; }
        public override decimal GeneratingSelfPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1893 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1893 - 1875) / 10.0M; }

        public override decimal GeneratingFeedingPowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1895 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1895 - 1875) / 10.0M; }
        public override decimal GeneratingFeedingPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1897 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1897 - 1875) / 10.0M; }
        public override decimal GeneratingFeedingPowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1899 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1899 - 1875) / 10.0M; }
        public override decimal GeneratingFeedingPowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1901 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1901 - 1875) / 10.0M; }
        public override decimal GeneratingFeedingPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1903 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1903 - 1875) / 10.0M; }

        public override decimal GeneratingFetchingPowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1905 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1905 - 1875) / 10.0M; }
        public override decimal GeneratingFetchingPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1907 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1907 - 1875) / 10.0M; }
        public override decimal GeneratingFetchingPowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1909 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1909 - 1875) / 10.0M; }
        public override decimal GeneratingFetchingPowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1911 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1911 - 1875) / 10.0M; }
        public override decimal GeneratingFetchingPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1913 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1913 - 1875) / 10.0M; }

        public override decimal GeneratingLoadPowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1915 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1915 - 1875) / 10.0M; }
        public override decimal GeneratingLoadPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1917 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1917 - 1875) / 10.0M; }
        public override decimal GeneratingLoadPowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1919 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1919 - 1875) / 10.0M; }
        public override decimal GeneratingLoadPowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1921 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1921 - 1875) / 10.0M; }
        public override decimal GeneratingLoadPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1923 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1923 - 1875) / 10.0M; }

        public override decimal GeneratingPvPowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1925 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1925 - 1875) / 10.0M; }
        public override decimal GeneratingPvPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1927 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1927 - 1875) / 10.0M; }
        public override decimal GeneratingPvPowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1929 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1929 - 1875) / 10.0M; }
        public override decimal GeneratingPvPowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1931 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1931 - 1875) / 10.0M; }
        public override decimal GeneratingPvPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1933 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1933 - 1875) / 10.0M; }

        public override decimal GeneratingBatteryDischargePowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1935 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1935 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryDischargePowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1937 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1937 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryDischargePowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1939 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1939 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryDischargePowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1941 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1941 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryDischargePowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1943 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1943 - 1875) / 10.0M; }

        public override decimal GeneratingBatteryChargePowerHourly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1945 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1945 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryChargePowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1947 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1947 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryChargePowerMonthly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1949 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1949 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryChargePowerYearly { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1951 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1951 - 1875) / 10.0M; }
        public override decimal GeneratingBatteryChargePowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1953 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1953 - 1875) / 10.0M; }
        public override decimal GeneratingPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1955 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1955 - 1875) / 10.0M; }
        public override decimal GeneratingPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1957 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1957 - 1875) / 10.0M; }

        public override decimal GeneratingAcDcPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1959 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1959 - 1875) / 10.0M; }
        public override decimal GeneratingAcDcPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1961 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1961 - 1875) / 10.0M; }

        public override decimal GeneratingDcAcPowerDaily { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1963 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1963 - 1875) / 10.0M; }
        public override decimal GeneratingDcAcPowerTotal { set { Array.Copy((value * 10.0M).ToUInt16s(), 0, _holding1875, 1965 - 1875, 2); } get => (EnableWriteGeneratings ? _holding1875 : Buff_Holding1875).GetPointU32(1965 - 1875) / 10.0M; }

        public IAsyncRelayCommand EnableWriteGeneratingCommand { get; set; }
        private async Task EnableWriteGenerating()
        {
            EnableWriteGeneratings = !EnableWriteGeneratings;
            if (EnableWriteGeneratings)
            {
                _holding1875 = Buff_Holding1875.Slice(1875 - 1875, 92).ToArray();
            }
        }

        public IAsyncRelayCommand WriteGeneratingCommand { get; set; }
        private async Task WriteGenerating()
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
                var buff = _holding1875.GetBytesFromBigEndian();
                await modbus.WriteHoldingRegisters(modbus.Options.SlaveId, 1875, buff);


                if (CommunicateAdapterPool.StoragePool.TryGetValue(SelectedLink, out var storage))
                {
                    storage.ClearBuffers();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                @lock.Release();
                EnableWriteGeneratings = false;
            }


        }

        public IAsyncRelayCommand ResetGeneratingCommand { get; set; }
        private async Task ResetGenerating()
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
                    StartingAddress = 1875,
                    Count = 92
                });

                if (CommunicateAdapterPool.StoragePool.TryGetValue(SelectedLink, out var storage))
                {
                    storage.ClearBuffers();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                @lock.Release();
            }


        }

        protected override void Holding1875Refreshing(UshortMessage message)
        {
            if (!EnableWriteGeneratings)
            {
                base.Holding1875Refreshing(message);

                Buff_Holding1875 = message.Buffer;
            }
        }
    }
}
