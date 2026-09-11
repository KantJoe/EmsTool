using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class MqttEnvironmentConfig: EmsConnectionOption
    {
        public string Environment { get; set; }

        public string ClientId { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public int MqttVersion { get; set; }

        public bool AutoConnect { get; set; }
    }
}
