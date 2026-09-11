using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using org.Communication;
using org.Communication.Extensions;
using org.Models;
using org.Models.Messagings;
using org.Ui;
using org.Ui.ViewModels;
using org.Ui.Views;
using org.Utils.Global;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Product01.ViewModels
{
    public partial class Product01PointMapViewModel : PointMapBaseViewModel
    {
        public Product01PointMapViewModel()
            :base()
        {
            RefreshCommand = new AsyncRelayCommand(Refresh);
            EditPointCommand = new AsyncRelayCommand<object>(EditPoint);
            StopCommand = new AsyncRelayCommand(Stop);
            AddAlertCommand = new AsyncRelayCommand<object>(AddAlert);
        }

        private async Task EditPoint(object point)
        {
            if (point is PointViewModel vm
                && SelectedMapping.FunctionCode == 3)
            {
                var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
                await @lock?.WaitAsync();

                var p = point as PointViewModel;
                p.Key = SelectedLink;
                await UiGlobalContext.ShowRootDialog(new EditPointDialog(p),
                    closedHandler: new DialogClosedEventHandler(EditPointClosedEvent));
            }
        }

        private async void EditPointClosedEvent(object sender, DialogClosedEventArgs e)
        {
            try
            {

                if (e.Parameter is not PointViewModel point)
                {
                    return;
                }

                if (!CommunicateAdapterPool.ModbusMasterPool.TryGetValue(point.Key, out var value)
                    || value?.Online != true
                    || value is not IModbusObject modbusObj)
                {
                    return;
                }

                var values = point.NextValue.Split(",", StringSplitOptions.RemoveEmptyEntries);
                byte[] buff = default;
                switch (point.DateType)
                {
                    case "ushort":
                        buff = new byte[values.Length * 2];
                        for (var index = 0; index < values.Length; index++)
                        {
                            if (ushort.TryParse(values[index], out ushort val))
                            {
                                buff[index * 2] = (byte)((val >> 8) & 0xFF);
                                buff[index * 2 + 1] = (byte)(val & 0xFF);
                            }
                        }
                        break;
                    case "uint32":
                        buff = new byte[values.Length * 4];
                        for (var index = 0; index < values.Length; index++)
                        {
                            if (UInt32.TryParse(values[index], out UInt32 val))
                            {
                                buff[index * 4] = (byte)((val >> 24) & 0xFF);
                                buff[index * 4 + 1] = (byte)((val >> 16) & 0xFF);
                                buff[index * 4 + 2] = (byte)((val >> 8) & 0xFF);
                                buff[index * 4 + 3] = (byte)(val & 0xFF);
                            }
                        }
                        break;
                    case "int32":
                        buff = new byte[values.Length * 4];
                        for (var index = 0; index < values.Length; index++)
                        {
                            if (Int32.TryParse(values[index], out Int32 val))
                            {
                                buff[index * 4] = (byte)((val >> 24) & 0xFF);
                                buff[index * 4 + 1] = (byte)((val >> 16) & 0xFF);
                                buff[index * 4 + 2] = (byte)((val >> 8) & 0xFF);
                                buff[index * 4 + 3] = (byte)(val & 0xFF);
                            }
                        }
                        break;
                    case "ASCII":
                    default:
                        buff = new byte[point.PointCount * 2];
                        var str = System.Text.Encoding.ASCII.GetBytes(point.NextValue);
                        Array.Copy(str, buff, Math.Min(str.Length, point.PointCount));
                        break;
                }

                await modbusObj.WriteHoldingRegisters(
                    modbusObj.Options.SlaveId, (ushort)point.Address, buff.AsMemory());
                if (CommunicateAdapterPool.StoragePool.TryGetValue(point.Key, out var storage))
                {
                    storage.ClearBuffers();
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "ModbusPointViewModel.EditPointClosedEvent");
            }
            finally
            {
                var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
                @lock?.Release();
            }
        }


        private async Task AddAlert(object obj)
        {
            if (obj is not PointViewModel vm)
            {
                return;
            }

            // 不检查是否存在重复告警项，强制覆盖
            var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
            await @lock?.WaitAsync();

            var p = new EmsDevicePointAlert
            {
                FunctionCode = (ModbusFunctionCode)vm.FunctionCode,
                StartAddress = vm.Address,
                PointCount = 1,
                ThresholdRangeValue = vm.CurrentValue.ToString()
            };

            await UiGlobalContext.ShowRootDialog(new EditPointAlertDialog(p),
                closedHandler: new DialogClosedEventHandler(EditPointAlertClosedEvent));
        }

        private async void EditPointAlertClosedEvent(object sender, DialogClosedEventArgs e)
        {
            try
            {

                if (e.Parameter is not EmsDevicePointAlert point
                    || !CommunicateAdapterPool.StoragePool.TryGetValue(SelectedLink, out var value))
                {
                    return;
                }

                var solution = EmsSolutionContext.Current;
                var device = solution.DeviceTopologies
                    .FirstOrDefault(f => f.ConnectionOptions.GetKey() == SelectedLink);
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
            finally
            {
                var @lock = ConsistencyContext.Instance.GetInstance(lockKey: SelectedLink);
                @lock?.Release();
            }
        }

        private async Task Stop()
        {
            ClearRegisters();
        }

        private async Task Refresh()
        {
            ClearRegisters();
            Initialize();

            WeakReferenceMessenger.Default
                .Register<PointMapBaseViewModel, UshortMessage, string>(this, SelectedLink, ReceiveMessage);
        }

        public override void Initialize()
        {
            InitializeLinks();
            InitializeGroups();
        }

        private void InitializeGroups()
        {
            var oldSelected = SelectedMapping?.Display;

            var solution = EmsSolutionContext.Current;
            Mappings = [.. solution.Settings.PointMapMappings
                .Select(s => new PointMapRangMappingViewModel(s))];
            if (!string.IsNullOrEmpty(oldSelected))
            {
                SelectedMapping = Mappings?.FirstOrDefault(f => f.Display == oldSelected)
                    ?? Mappings.FirstOrDefault();
            }
        }

        private void InitializeLinks()
        {
            var oldSelected = SelectedLink;
            // 仅显示modbus对象，mqtt忽略
            Links = [.. CommunicateAdapterPool.ModbusMasterPool
                .Where(w=>w.Value is IModbusObject)
                .Select(s => s.Key)];
            if (!string.IsNullOrEmpty(oldSelected))
            {
                SelectedLink = Links?.FirstOrDefault(f => f == oldSelected)
                    ?? Links.FirstOrDefault();
            }
        }

        public override void ClearRegisters()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
