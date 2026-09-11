using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace org.Models
{
    public class GenericConfig
    {
        public string MainTitle { get; set; }

        public string SubTitle { get; set; }

        public int MajorVersion { get; set; }

        public int MinorVersion { get; set; }

        public int BuildVersion { get; set; }

        public string Revision { get; set; }

        public string EmsServer { get; set; }

        public string EmsServerPort { get; set; }

        public string CheckUpdateUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string AdminDefaultPwd { get; set; }
        /// <summary>
        /// 交互-实时渲染间隔:默认5000ms
        /// </summary>
        public int LivingRenderInterval { get; set; }

        #region 其它
        public bool Others_CheckUpdate { get; set; }

        #endregion

        [JsonIgnore]
        public string Version => GetVersion();
        
        public MqttConfig MqttConfig { get; set; }

        public MultiLangConfig MultipleLang { get; set; }

        public DatabaseConfig DatabaseConfig { get; set; }

        private string GetVersion()
        {
            var rev = string.IsNullOrEmpty(Revision) ? "" : ("-" + Revision);
            return $"{MajorVersion}.{MinorVersion}.{BuildVersion}{rev}";
        }
    }
}
