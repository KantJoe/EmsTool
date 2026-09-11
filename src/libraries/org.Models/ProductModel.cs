using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class ProductModel
    {
        public string ModelName { get; set; }
        /// <summary>
        /// 电池类型
        /// </summary>
        public string BatteryType { get; set; }
        /// <summary>
        /// 最小系统容量 Wh
        /// </summary>
        public decimal? MinSystemEnergy { get; set; }
        /// <summary>
        /// 最大系统容量 Wh
        /// </summary>
        public decimal? MaxSystemEnergy { get; set; }
        /// <summary>
        /// 模块电压 V
        /// </summary>
        public decimal? ModuleVoltage { get; set; }
        /// <summary>
        /// 模块容量 Ah
        /// </summary>
        public decimal? ModuleCapacity { get; set; }
        /// <summary>
        /// 长 mm
        /// </summary>
        public decimal? Width { get; set; }
        /// <summary>
        /// 宽 mm
        /// </summary>
        public decimal? Deepth { get; set; }
        /// <summary>
        /// 高 mm
        /// </summary>
        public decimal? Height { get; set; }
        /// <summary>
        /// 防尘防水等级
        /// </summary>
        public string IngressProtection { get; set; }

        public string LocalPicturePath { get; set; }
        /// <summary>
        /// 循环寿命
        /// </summary>
        public string RecycleLifetime { get; set; }
        /// <summary>
        /// 系统标称容量
        /// </summary>
        public decimal? SoC { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }
    }
}
