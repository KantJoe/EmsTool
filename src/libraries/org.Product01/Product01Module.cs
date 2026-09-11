using CommunityToolkit.Mvvm.Messaging;
using org.Product01.ViewModels;
using org.Product01.Views;
using org.Models;
using org.Models.Messagings;
using org.Ui;
using org.Ui.ViewModels;
using org.Ui.Views;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace org.Product01
{
    public class Product01Module : IModule, IMenuControl, IMenuViewModel
    {
        public string Key { get; private set; }

        public Product01Module()
        {
            Key = EmsProtocol.Product01;
        }

        public void Load()
        {
            ModuleContext.Instance.ModulePool[Key] = this;

            var settings = ResetEmsSetting();
            foreach (var mapping in settings.PointMapMappings)
            {
                var key = $"{(int)mapping.FunctionCode}_{mapping.StartAddress}";
                ModbusGroupContext.Instance.Groups[key]
                    = (buff, token) => WeakReferenceMessenger.Default
                    .Send<UshortMessage, string>(new UshortMessage(
                        buff, key), token);
            }
        }

        public void Unload()
        {
            ModuleContext.Instance.ModulePool.Remove(EmsProtocol.Product01);
        }

        public UserControl CreateDeviceTopologyControl(object obj)
        {
            return new EnergyMovingOfProduct01Control();
        }

        public EmsSettings ResetEmsSetting()
        {
            var settings = new EmsSettings
            {
                PointMapMappings = new List<PointMapRangMapping>()
            };
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "系统参数1",
                MapLimitation = 0,
                StartAddress = 0
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "系统参数2",
                MapLimitation = 0,
                StartAddress = 125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "时间点使能控制1",
                MapLimitation = 0,
                StartAddress = 250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "采集器描述",
                MapLimitation = 0,
                StartAddress = 375
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "尖峰平谷时段表",
                MapLimitation = 0,
                StartAddress = 500
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "调试模式设置",
                MapLimitation = 0,
                StartAddress = 625
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "APN参数",
                MapLimitation = 0,
                StartAddress = 750
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "PCS数据",
                MapLimitation = 0,
                StartAddress = 1000
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "DCDC数据",
                MapLimitation = 0,
                StartAddress = 1125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "STS数据",
                MapLimitation = 1,
                StartAddress = 1250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "消防数据",
                MapLimitation = 0,
                StartAddress = 1375
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "空调数据",
                MapLimitation = 0,
                StartAddress = 1500
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "液冷数据",
                MapLimitation = 0,
                StartAddress = 1625
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "温湿度数据",
                MapLimitation = 0,
                StartAddress = 1750
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "发电量数据",
                MapLimitation = 0,
                StartAddress = 1875
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "时间点使能控制2",
                MapLimitation = 0,
                StartAddress = 2000
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "时间点使能控制3",
                MapLimitation = 0,
                StartAddress = 2125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "时间点使能控制4",
                MapLimitation = 0,
                StartAddress = 2250
            });

            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "系统数据",
                MapLimitation = 0,
                StartAddress = 0
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "实时数据1",
                MapLimitation = 0,
                StartAddress = 125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "并机数据1",
                MapLimitation = 0,
                StartAddress = 250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "故障历史",
                MapLimitation = 1,
                StartAddress = 375
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "PCS数据",
                MapLimitation = 0,
                StartAddress = 500
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "DCDC数据",
                MapLimitation = 0,
                StartAddress = 625
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据1",
                MapLimitation = 0,
                StartAddress = 750
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据2",
                MapLimitation = 0,
                StartAddress = 875
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据3",
                MapLimitation = 0,
                StartAddress = 1000
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "电表数据",
                MapLimitation = 0,
                StartAddress = 1125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "STS数据",
                MapLimitation = 0,
                StartAddress = 1250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "消防数据",
                MapLimitation = 0,
                StartAddress = 1375
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "空调数据",
                MapLimitation = 0,
                StartAddress = 1500
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "液冷数据",
                MapLimitation = 0,
                StartAddress = 1625
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "温湿度数据",
                MapLimitation = 0,
                StartAddress = 1750
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "尖峰平谷数据1",
                MapLimitation = 0,
                StartAddress = 1875
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "发电量数据1",
                MapLimitation = 0,
                StartAddress = 2000
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "4G模块",
                MapLimitation = 0,
                StartAddress = 2125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "并机数据2",
                MapLimitation = 1,
                StartAddress = 4000
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "实时数据2",
                MapLimitation = 1,
                StartAddress = 4125
            });

            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "尖峰平谷数据2",
                MapLimitation = 1,
                StartAddress = 5875
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "发电量数据2",
                MapLimitation = 1,
                StartAddress = 6000
            });

            return settings;
        }

        public PointMapBaseViewModel PointMapInitialize()
        {
            return new Product01PointMapViewModel();
        }

        public UserControl CreateSystemSummaryControl(object obj)
        {
            return new SummaryOfProduct01Control();
        }

        public SystemSettingViewModelBase SystemSettingViewModelInitialize()
        {
            return default;
        }
    }
}
