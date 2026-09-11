using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    /// <summary>
    /// 点表数据映射
    /// 表内的点必定连续
    /// 点表范围:startAddress+PointCount= [startAddress,endAddress]
    /// </summary>
    public class PointMapRangMapping
    {

        public ModbusFunctionCode FunctionCode { get; set; }

        /// <summary>
        /// 表的读取限制
        /// 0：无限制
        /// 1：并网后开放读取
        /// -1: 不存在
        /// </summary>
        public int MapLimitation { get; set; }

        public int StartAddress { get; set; }

        public int PointCount { get; set; } = 125;

        public string GroupName { get; set; }
    }
}
