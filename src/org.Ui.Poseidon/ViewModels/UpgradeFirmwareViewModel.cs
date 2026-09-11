using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using org.Communication;
using org.DomainService.Firmware;
using org.Models.Messagings;
using org.Ui.Messagings;
using org.Ui.Poseidon.Views;
using org.Ui.Views;
using org.Utils.Global;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class UpgradeFirmwareViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _logs;

        [ObservableProperty]
        private ObservableCollection<string> _links;

        [ObservableProperty]
        private string _selectedLink;

        [ObservableProperty]
        private double _progressValue;

        [ObservableProperty]
        private bool _upgrading;

        [ObservableProperty]
        private bool _modbusLinking;

        [ObservableProperty]
        private Input00Message _message;

        [ObservableProperty]
        private string _firmwareFilePath;

        public WeakReference LogsScrollToEnd { get; set; }

        public UpgradeFirmwareViewModel()
        {
            SelectFirmwareCommand = new AsyncRelayCommand(SelectFirmware);
            UpgradeCommand = new AsyncRelayCommand(Upgrade);
            InterruptUpgradeCommand = new AsyncRelayCommand(InterruptUpgrade);

            Logs = [];

            var devices = EmsSolutionContext.Current.DeviceTopologies
                ?.Select(s => s.CommunicateType == Models.CommunicateType.ModbusRtu
                    ? s.ConnectionOptions.PortName : $"{s.ConnectionOptions.Ip}_{s.ConnectionOptions.Port}")
                ?? [];
            Links = [.. devices];
            SelectedLink = Links.FirstOrDefault();
        }

        public IAsyncRelayCommand SelectFirmwareCommand { get; set; }
        private async Task SelectFirmware()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Ems固件(*.bin)|*.bin";

            if (ofd.ShowDialog() != true || !File.Exists(ofd.FileName))
            {
                return;
            }

            FirmwareFilePath = ofd.FileName;

        }

        public IAsyncRelayCommand UpgradeCommand { get; set; }
        private async Task Upgrade()
        {
            if (string.IsNullOrEmpty(SelectedLink)
                || !File.Exists(FirmwareFilePath))
            {
                return;
            }

            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var client)
                || !client.Online)
            {
                return;
            }

            if (ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink) is not SemaphoreSlim @lock)
            {
                return;
            }

            try
            {
                await @lock.WaitAsync(-1);

                Upgrading = true;
                ProgressValue = 0;
                var buffer = await File.ReadAllBytesAsync(FirmwareFilePath);

                var timeout = TimeSpan.FromMinutes(5);
                var source = new CancellationTokenSource(timeout);
                var response = await new GenericUpgradeService()
                    .BurnFirmwareFlowAsync(
                        client, ProgressUpdateCallback, buffer, UiGlobalContext.Instance.MainSlaveId, source.Token);

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "");
            }
            finally
            {
                @lock.Release();

                Upgrading = false;
            }

        }

        private void ProgressUpdateCallback(string msg, double target)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Logs.Add($"[{DateTime.Now:HH:mm:ss}] {msg}");
            }

            if (ProgressValue >= 100)
            {
                return;
            }

            if (target <= 0)
            {
                return;
            }

            if (target >= 100 || ProgressValue >= 100)
            {
                ProgressValue = 100;
            }

            ProgressValue = target;
        }


        public IAsyncRelayCommand InterruptUpgradeCommand { get; set; }
        private async Task InterruptUpgrade()
        {

        }

        public void Loaded(Action action)
        {
            LogsScrollToEnd = new WeakReference(action);

            (LogsScrollToEnd.Target as Action).Invoke();
        }

        public void Unloaded()
        {
            LogsScrollToEnd = null;
        }

        public async Task Receive(Input00Message message)
        {
            Message = message;

        }
    }
}
