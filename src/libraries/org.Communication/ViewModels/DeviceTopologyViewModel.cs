using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using org.Communication.Extensions;
using org.Models;
using org.Models.Messagings;
using org.Ui;
using org.Ui.Messagings;
using org.Ui.ViewModels;
using org.Ui.Views;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TouchSocket.Core;
using TouchSocket.Modbus;

namespace org.Communication.ViewModels
{
    public partial class DeviceTopologyViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<EmsNetworkEndpointViewModel> _items;

        [ObservableProperty]
        private EmsSolution _solution;

        [ObservableProperty]
        private ProductModelViewModel _firstDeviceModel;

        /// <summary>
        /// 视图对象
        /// </summary>
        [ObservableProperty]
        private object _energyMovingTopology;

        [ObservableProperty]
        private decimal? _chargingCurrent;

        [ObservableProperty]
        private decimal? _disChargingCurrent;

        [ObservableProperty]
        private ObservableCollection<string> _links;

        [ObservableProperty]
        private string _selectedLink;

        public DeviceTopologyViewModel()
        {
            ConnectAllCommand = new AsyncRelayCommand(ConnectAll);
            DisconnectAllCommand = new AsyncRelayCommand(DisconnectAll);
            ConnectOneCommand = new AsyncRelayCommand<object>(ConnectOne);

            Solution = EmsSolutionContext.Current;
            var endPoints = (Solution?.DeviceTopologies?.Select(
                    s => new EmsNetworkEndpointViewModel(s))
                ?? new List<EmsNetworkEndpointViewModel>())
                .OrderBy(o => o.OrderNumber);
            foreach (var option in endPoints)
            {
                if (CommunicateAdapterPool.ModbusMasterPool.TryGetValue(
                        option.Options?.GetOptions()?.GetKey(), out IConnectClient client))
                {
                    option.SetModbusObject(client);
                }
            }

            Items = [.. endPoints];
            EnergyMovingTopology = InitialEnergyMovingContent(Solution?.DeviceTopologies?.FirstOrDefault()?.EmsProtocol);
        }

        public IAsyncRelayCommand ConnectAllCommand { get; set; }
        private async Task ConnectAll()
        {
            try
            {
                SelectedLink = null;
                await DisconnectAll();
                await CommunicateAdapterPool.ClearModbusMastersAsync();

                // TODO: validations
                foreach (var item in Items)
                {
                    if (string.IsNullOrEmpty(item?.Options?.PortName))
                    {
                        MessageBox.Show("存在串口号为空");
                        return;
                    }
                }

                foreach (var endPointVM in Items)
                {
                    var options = endPointVM?.Options?.GetOptions();
                    var key = options?.GetKey();

                    switch (endPointVM.CommunicateType)
                    {
                        case CommunicateType.ModbusRtu:
                            {
                                if (endPointVM?.ModbusObject is null
                                    || !CommunicateAdapterPool.ModbusMasterPool.ContainsKey(key))
                                {
                                    endPointVM.SetModbusObject(
                                        await CommunicateAdapterPool
                                            .AddRtuAsync(options));
                                }
                                break;
                            }
                        case CommunicateType.ModbusTcp:
                            {
                                if (endPointVM?.ModbusObject is null
                                    || !CommunicateAdapterPool.ModbusMasterPool.ContainsKey(key))
                                {
                                    endPointVM.SetModbusObject(
                                        await CommunicateAdapterPool
                                            .AddTcpAsync(options));
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }

                await CommunicateAdapterPool.ReconnectModbusMastersAsync();

                await EmsSolutionContext.SaveEmsSolution(EmsSolutionContext.Current);

                InitializeLinks();
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "DeviceTopologyViewModel.ConnectAll");
                MessageBox.Show("连接失败，请检查设备上电及连接参数后重试");
            }
        }

        public IAsyncRelayCommand<object> ConnectOneCommand { get; set; }
        private async Task ConnectOne(object obj)
        {
            if (obj is not EmsNetworkEndpointViewModel endPointVM)
            {
                return;
            }

            try
            {
                SelectedLink = null;

                #region 清理因编辑COM口、ip&port而脱离映射关系的连接
                var currentKeys = Items.Select(s => s.Options.GetOptions().GetKey());
                var notEsistConns = CommunicateAdapterPool.ModbusMasterPool
                    .Where(w => !currentKeys.Contains(w.Key));
                foreach (var item in notEsistConns)
                {
                    await item.Value.DisconnectAsync();
                    CommunicateAdapterPool.ModbusMasterPool.Remove(item.Key);
                }
                #endregion

                #region 重新构建连接，防止配置不一致
                var options = endPointVM?.Options?.GetOptions();
                var key = options?.GetKey();
                switch (endPointVM.CommunicateType)
                {
                    case CommunicateType.ModbusRtu:
                        {
                            if (endPointVM?.ModbusObject is null
                                || !CommunicateAdapterPool.ModbusMasterPool.ContainsKey(key))
                            {
                                endPointVM.SetModbusObject(
                                    await CommunicateAdapterPool
                                        .AddRtuAsync(options));
                            }
                            break;
                        }
                    case CommunicateType.ModbusTcp:
                        {
                            if (endPointVM?.ModbusObject is null
                                || !CommunicateAdapterPool.ModbusMasterPool.ContainsKey(key))
                            {
                                endPointVM.SetModbusObject(
                                    await CommunicateAdapterPool
                                        .AddTcpAsync(options));
                            }
                            break;
                        }
                    default:
                        break;
                }
                #endregion

                #region 重连
                var storage = CommunicateAdapterPool.StoragePool[key];
                storage.ClearBuffers();

                var connection = CommunicateAdapterPool.ModbusMasterPool[key];
                await connection.DisconnectAsync();
                await connection.ConnectAsync();
                #endregion

                #region 刷新ui
                UiGlobalContext.Instance.MainSlaveId = CommunicateAdapterPool.ModbusMasterPool.Values.Min(m => m.Options.SlaveId);
                #endregion

                #region 保存配置
                await EmsSolutionContext.SaveEmsSolution(EmsSolutionContext.Current);
                #endregion

                #region 刷新绑定的能量流信息
                InitializeLinks();
                #endregion
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "DeviceTopologyViewModel.ConnectOne");
                MessageBox.Show("连接失败，请检查设备上电及连接参数后重试");
            }
        }

        public IAsyncRelayCommand DisconnectAllCommand { get; set; }
        private async Task DisconnectAll()
        {
            SelectedLink = null;
            //await CommunicateAdapterPool.DisconnectAllMqttClientsAsync();
            await CommunicateAdapterPool.DisconnectAllModbusMastersAsync();
            InitializeLinks();
        }

        [RelayCommand]
        private void AddRtuConnection()
        {
            var no = Solution.DeviceTopologies.Count + 1;
            var option = EmsNetworkEndpointViewModel.NewRtuOptions(no, (byte)no);
            var device = new EmsDevice
            {
                Alias = option.PortName,
                EmsProtocol = Items?.FirstOrDefault()?.SelectedProtocol,
                OrderNumber = (Solution.DeviceTopologies?.Count ?? 0) + 1,
                CommunicateType = CommunicateType.ModbusRtu,
                ConnectionOptions = option
            };
            Solution.DeviceTopologies.Add(device);

            Items.Add(new EmsNetworkEndpointViewModel(device));
            InitializeLinks();
        }

        [RelayCommand]
        private void DeleteRtu(object rtu)
        {
            var rtuVM = rtu as EmsNetworkEndpointViewModel;
            Items.Remove(rtuVM);
            Solution.DeviceTopologies.Remove(rtuVM.Device);
            InitializeLinks();
        }

        [RelayCommand]
        private void AddTcpConnection()
        {
            var option = EmsNetworkEndpointViewModel.NewTcpOptions(Solution.DeviceTopologies.Count + 1);
            var device = new EmsDevice
            {
                EmsProtocol = Items?.FirstOrDefault()?.SelectedProtocol,
                OrderNumber = (Solution.DeviceTopologies?.Count ?? 0) + 1,
                CommunicateType = CommunicateType.ModbusTcp,
                ConnectionOptions = option
            };
            Solution.DeviceTopologies.Add(device);

            Items.Add(new EmsNetworkEndpointViewModel(device));
            InitializeLinks();
        }

        [RelayCommand]
        private void DeleteTcp(object tcp)
        {
            var tcpVM = tcp as EmsNetworkEndpointViewModel;
            Items.Remove(tcpVM as EmsNetworkEndpointViewModel);
            Solution.DeviceTopologies.Remove(tcpVM.Device);
            InitializeLinks();
        }

        private UserControl InitialEnergyMovingContent(string productModel)
        {


            var module = (IMenuControl)ModuleContext.Instance.GetModule(productModel);
            return module is null ? default : module.CreateDeviceTopologyControl(null);
        }

        public void InitializeLinks()
        {
            var list = Items.Select(s => s.CommunicateType == CommunicateType.ModbusRtu
                ? s.Options.PortName : $"{s.Options.Ip}_{s.Options.Port}").ToList();
            var outerList = CommunicateAdapterPool.ModbusMasterPool.Keys
                .Where(w => !list.Any(a => a == w)).ToList();
            list = list.Concat(outerList).ToList();
            Links = [.. list];
        }
    }
}
