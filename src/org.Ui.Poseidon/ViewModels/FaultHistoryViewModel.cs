using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Dm;
using org.Communication;
using org.Ui.MultiLanguage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using org.Utils;
using org.Utils.Global;
using Microsoft.Win32;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class FaultHistoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _links;

        [ObservableProperty]
        private string _selectedLink;

        [ObservableProperty]
        private ObservableCollection<FaultItemViewModel> _faultItems;

        [ObservableProperty]
        private FaultItemViewModel _selectedItem;

        public FaultHistoryViewModel()
        {
            RefreshCommand = new AsyncRelayCommand(Refresh);
            SaveAsCommand = new AsyncRelayCommand(SaveAs);
        }

        public IAsyncRelayCommand RefreshCommand { get; set; }
        private async Task Refresh()
        {
            if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(SelectedLink, out var modbus)
                || modbus?.Online != true
                || modbus is not IModbusObject client)
            {
                return;
            }

            if (ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink) is not SemaphoreSlim @lock)
            {
                return;
            }

            var list = new List<FaultItemViewModel>();
            var emsSetting = EmsSolutionContext.Current?.Settings;
            try
            {
                await @lock.WaitAsync(-1);

                var no = 0;
                while (no++ < 30)
                {
                    var data = await client.ReadInputRegisters(modbus.Options.SlaveId, 375, 32);
                    var tmpNo = data.GetPoint(0);
                    if (tmpNo == 0 || list.Any(a => a.No == tmpNo))
                    {
                        break;
                    }

                    list.Add(new FaultItemViewModel(data));

                    if (emsSetting?.ModbusPollingOptimizeSwitch != true)
                    {
                        await Task.Delay(emsSetting?.ModbusPollingOptimizeValue ?? 3);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "FaultHistoryViewModel.Refresh");
            }
            finally
            {
                @lock.Release();

                FaultItems = [.. list.OrderBy(o => o.No)];
            }
        }

        public IAsyncRelayCommand SaveAsCommand { get; set; }
        private async Task SaveAs()
        {

            var dialog = new SaveFileDialog();
            dialog.FileName = "Ems Fault History.csv";
            dialog.Filter = "(*.csv)|*.csv";
            if (dialog.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("No,EventCode,EventLevel,TriggedDateTime,Content\r\n");
                foreach (var item in FaultItems)
                {
                    sb.Append($"{item.No},{item.EventCode},{item.EventLevelStr},{item.TriggedDateTime},{item.Content}\r\n");
                }

                await System.IO.File.WriteAllTextAsync(dialog.FileName, sb.ToString(), Encoding.UTF8);
            }
        }



        public void Load()
        {
            var list = CommunicateAdapterPool.ModbusMasterPool.Keys
                .ToList();
            Links = [.. list];
            if (Links?.Any() is true)
            {
                SelectedLink = Links.First();
            }
        }

        public void Unload()
        {
        }

    }
}
