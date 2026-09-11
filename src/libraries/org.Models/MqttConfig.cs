using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class MqttConfig
    {
        public List<MqttEnvironmentConfig> EnvironmentConfigs { get; set; }
        public List<string> DefaultTopics { get; set; }
        /// <summary>
        /// 轮询间隔（ms)
        /// </summary>
        public int PollingInterval { get; set; }

        /// <summary>
        /// 采集器型号
        /// </summary>
        public string CollectorModel { get; set; }

        /// <summary>
        /// 采集器品牌
        /// </summary>
        public string CollectorBrand { get; set; }
        /// <summary>
        /// 采集器固件版本
        /// </summary>
        public string CollectorVersion { get; set; }
    }
}
