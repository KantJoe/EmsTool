using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    /// <summary>
    /// 告警
    /// </summary>
    public class EmsDevicePointAlert
    {
        public ModbusFunctionCode FunctionCode { get; set; }
        public int StartAddress { get; set; }
        /// <summary>
        /// 连续点数量
        /// </summary>
        public int PointCount { get; set; }

        public AlertCondition Condition { get; set; }

        public AlertTimes AlertTimes { get; set; }

        /// <summary>
        /// 范围值：left~right
        /// 指定值：value
        /// </summary>
        public string ThresholdRangeValue { get; set; }

        public ushort Value
        {
            get
            {
                ushort.TryParse(ThresholdRangeValue, out var val);
                return val;
            }
        }

        public ushort LeftValue
        {
            get
            {
                var vals = ThresholdRangeValue.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (vals.Length > 1
                    && ushort.TryParse(vals[0], out var val))
                {
                    return val;
                }

                return 0;
            }
        }

        public ushort RightValue
        {
            get
            {
                var vals = ThresholdRangeValue.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (vals.Length > 1
                    && ushort.TryParse(vals[1], out var val))
                {
                    return val;
                }

                return 0;
            }
        }
    }
}
