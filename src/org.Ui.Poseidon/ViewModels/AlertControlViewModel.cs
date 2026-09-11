using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using org.Communication;
using org.Communication.Extensions;
using org.Models;
using org.Ui.ViewModels;
using org.Ui.Views;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class AlertControlViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ModbusKeyViewModel> _modbusKeys;

        [ObservableProperty]
        private ObservableCollection<PointAlertViewModel> _points;

        [ObservableProperty]
        private PointAlertViewModel _selectedPoint;

        public AlertControlViewModel()
        {
            DeletePointCommand = new AsyncRelayCommand<object>(DeletePoint);
            EditPointCommand = new AsyncRelayCommand<object>(EditPoint);
            AllowAlertCommand = new AsyncRelayCommand<object>(AllowAlert);

            var keyList = new List<ModbusKeyViewModel>();
            var pointList = new List<PointAlertViewModel>();
            foreach (var storage in CommunicateAdapterPool.StoragePool.Values)
            {
                keyList.Add(new ModbusKeyViewModel(storage.ModbusKey, storage.AllowAlert));

                pointList.AddRange(storage.AlertPoints
                    .Select(s => new PointAlertViewModel(s.Value, storage.ModbusKey)));
            }

            ModbusKeys = [.. keyList];
            Points = [.. pointList];
        }

        public IAsyncRelayCommand<object> DeletePointCommand { get; set; }
        private async Task DeletePoint(object obj)
        {
            if (obj is not PointAlertViewModel vm)
            {
                return;
            }

            try
            {
                var solution = EmsSolutionContext.Current;
                var device = solution?.DeviceTopologies
                    ?.FirstOrDefault(f => f?.ConnectionOptions?.GetKey() == vm.ModbusKey);
                device?.AlertPoints?.RemoveAll(ra => ra.FunctionCode == vm.Point.FunctionCode
                    && ra.StartAddress == vm.Point.StartAddress);

                await EmsSolutionContext.SaveEmsSolution(solution);
                if (CommunicateAdapterPool.StoragePool.TryGetValue(vm.ModbusKey, out var storage))
                {
                    storage.AlertPoints.Remove(
                        $"{(int)vm.Point.FunctionCode}_{vm.Point.StartAddress}", out _);
                }

                Points.Remove(vm);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"AlertControlViewModel.DeletePoint:{vm.ModbusKey}-{vm.Point.FunctionCode}-{vm.Point.StartAddress}");
            }
        }

        public IAsyncRelayCommand<object> EditPointCommand { get; set; }
        private async Task EditPoint(object obj)
        {
            if (obj is not PointAlertViewModel vm)
            {
                return;
            }

            try
            {
                SelectedPoint = vm;
                await UiGlobalContext.ShowRootDialog(new EditPointAlertDialog(vm.Point),
                    closedHandler: new DialogClosedEventHandler(EditPointClosedEvent));
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"AlertControlViewModel.EditPoint:{vm.ModbusKey}-{vm.Point.FunctionCode}-{vm.Point.StartAddress}");
            }
        }

        private async void EditPointClosedEvent(object sender, DialogClosedEventArgs e)
        {
            try
            {
                var modbusKey = SelectedPoint?.ModbusKey;
                if (e.Parameter is not EmsDevicePointAlert point
                    || !CommunicateAdapterPool.StoragePool.TryGetValue(modbusKey, out var value))
                {
                    return;
                }

                Points.Remove(SelectedPoint);
                Points.Add(new PointAlertViewModel(point, modbusKey));

                var solution = EmsSolutionContext.Current;
                var device = solution.DeviceTopologies
                    .FirstOrDefault(f => f.ConnectionOptions.GetKey() == modbusKey);
                if (device is null)
                {
                    return;
                }

                if (device.AlertPoints?.Any() != true)
                {
                    device.AlertPoints = [];
                }

                device.AlertPoints.RemoveAll(ra => ra.FunctionCode == point.FunctionCode
                    && ra.StartAddress == ra.StartAddress);
                device.AlertPoints.Add(point);
                value.AppendAlerts([point]);

                await EmsSolutionContext.SaveEmsSolution(solution);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "ModbusPointViewModel.EditPointAlertClosedEvent");
            }
        }

        public IAsyncRelayCommand<object> AllowAlertCommand { get; set; }
        private async Task AllowAlert(object obj)
        {
            if (obj is not ModbusKeyViewModel vm)
            {
                return;
            }

            try
            {
                if (CommunicateAdapterPool.StoragePool.TryGetValue(vm.Key, out var storage))
                {
                    storage.AllowAlert = vm.AllowAlert;
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"AlertControlViewModel.AllowAlert:{vm.Key}--{vm.AllowAlert}");
            }
        }
    }

    public partial class ModbusKeyViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _key;

        [ObservableProperty]
        private bool _allowAlert;

        public ModbusKeyViewModel(string key, bool allow)
        {
            Key = key;
            AllowAlert = allow;
        }
    }
}
