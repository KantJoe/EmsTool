using org.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.Utils.Global
{
    public class EmsSolutionContext
    {
        public static EmsSolution Current { get; private set; }

        public static List<string> Files { get; private set; }

        static EmsSolutionContext()
        {
            Files = new List<string>();
        }

        /// <summary>
        /// 初始化：
        /// 1. 维护ems解决方案存放目录
        /// 2. 清理非.emss文件
        /// </summary>
        public static void Reinitialize()
        {
            if (!Directory.Exists(FilePathConst.EmsSolutionDirectory))
            {
                Directory.CreateDirectory(FilePathConst.EmsSolutionDirectory);
            }
            RemoveOthers();
        }

        public static async Task<bool> ImportEmsSolution(string json)
        {
            try
            {
                var solution = Newtonsoft.Json.JsonConvert.DeserializeObject<EmsSolution>(json);
                if (Files.Any(a => Enumerable.SequenceEqual(a, solution.EmsSolutionName)))
                {
                    solution.EmsSolutionName += "_1";
                }

                return await SaveEmsSolution(solution);
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "EmsSolutionContext.ImportEmsSolution");
            }

            return false;
        }

        public static async Task<bool> SaveEmsSolution(EmsSolution emsSolution)
        {
            var filePath = Path.Combine(
                    FilePathConst.EmsSolutionDirectory,
                    emsSolution.EmsSolutionName + FilePathConst.EmsSolution_FileExt);
            await File.WriteAllTextAsync(
                filePath,
                Newtonsoft.Json.JsonConvert.SerializeObject(emsSolution, Formatting.Indented),
                Encoding.UTF8);

            Files.Remove(filePath);
            Files.Add(filePath);

            return true;
        }

        private static void RemoveOthers()
        {
            var files = Directory.GetFiles(FilePathConst.EmsSolutionDirectory, "*.*", SearchOption.TopDirectoryOnly);

            foreach (var path in files)
            {
                try
                {
                    if (Path.GetExtension(path) != FilePathConst.EmsSolution_FileExt)
                    {
                        File.Delete(path);

                        continue;
                    }

                    Files.Add(path);
                }
                catch (Exception ex)
                {
                    LogFactory.Error(ex, "EmsSolutionContext.Initialize.RemoveOthers");
                }
            }

        }


        public static void SetSolution(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var json = File.ReadAllText(filePath, Encoding.UTF8);
            try
            {
                SetSolution(Newtonsoft.Json.JsonConvert
                    .DeserializeObject<EmsSolution>(json));
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "EmsSolutionContext.SetSolution");
            }
        }

        public static void SetSolution(EmsSolution emsSolution)
        {
            EmsSolutionContext.Current = emsSolution;
        }

        public static EmsSolution CreateEmsSolution(string protocol, string solutionName)
        {
            var solution = new EmsSolution
            {
                EmsSolutionName = solutionName,
                DeviceTopologies = new List<EmsDevice>
                        {
                            new EmsDevice{
                                OrderNumber=1,
                                Alias = protocol,
                                EmsProtocol= protocol,
                                CommunicateType= CommunicateType.ModbusRtu,
                                ConnectionOptions= EmsConnectionOption.CreateDefaultRtuOptions()
                            }
                        },
                Settings = CreateDefaultEmsSettings(),
                DynamicModules = new List<string> { $"org.{protocol}.dll" }
            };

            return solution;
        }

        /// <summary>
        /// 参考stackx
        /// </summary>
        /// <returns></returns>
        private static EmsSettings CreateDefaultEmsSettings()
        {
            var settings = new EmsSettings
            {
                PointMapMappings = new List<PointMapRangMapping>()
            };
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode =ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "系统参数",
                MapLimitation=0,
                StartAddress=0
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "系统设置",
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
                GroupName = "BMS数据",
                MapLimitation = 0,
                StartAddress = 1125
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
                GroupName = "时间点是能控制4",
                MapLimitation = 0,
                StartAddress = 2250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "蓝牙匹配",
                MapLimitation = 0,
                StartAddress = 2375
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadHoldingRegisters,
                GroupName = "动态电价数据",
                MapLimitation = 1,
                StartAddress = 2500
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
                GroupName = "历史数据",
                MapLimitation = 0,
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
                GroupName = "电表数据",
                MapLimitation = 0,
                StartAddress = 1125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "电池匹配CAN",
                MapLimitation = 0,
                StartAddress = 1250
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
                GroupName = "蓝牙模块",
                MapLimitation = 0,
                StartAddress = 2250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "激活与验收1",
                MapLimitation = 0,
                StartAddress = 2500
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "激活与验收2",
                MapLimitation = 0,
                StartAddress = 2625
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据1",
                MapLimitation = 0,
                StartAddress = 3000
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据2",
                MapLimitation = 0,
                StartAddress = 3125
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据3",
                MapLimitation = 0,
                StartAddress = 3250
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS数据4",
                MapLimitation = 0,
                StartAddress = 3375
            });
            settings.PointMapMappings.Add(new PointMapRangMapping
            {
                FunctionCode = ModbusFunctionCode.ReadInputRegisters,
                GroupName = "BMS-Pack数据",
                MapLimitation = 0,
                StartAddress = 3500
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

        public static void RemoveSolution(string solutionName)
        {
            var filePath = Path.Combine(
                    FilePathConst.EmsSolutionDirectory,
                    solutionName + FilePathConst.EmsSolution_FileExt);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            Files.Remove(filePath);
            if (solutionName == Current?.EmsSolutionName)
            {
                SetSolution(default(EmsSolution));
            }
        }
    }
}
