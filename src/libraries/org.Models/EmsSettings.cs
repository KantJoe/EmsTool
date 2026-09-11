using System.Collections.Generic;

namespace org.Models
{
    public class EmsSettings
    {
        /// <summary>
        /// 轮询间隔：ms
        /// </summary>
        public int ModbusPollingInterval { get; set; } = 2000;

        public bool ModbusPollingOptimizeSwitch { get; set; }

        public int ModbusPollingOptimizeValue { get; set; } = 3;

        public int LengthOfPointMapGroup { get; set; } = 125;

        public int LivingTracingMinimumInterval { get; set; } = 50;
        public List<PointMapRangMapping> PointMapMappings { get; set; }
    }
}