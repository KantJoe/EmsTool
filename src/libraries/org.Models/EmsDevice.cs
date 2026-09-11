using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class EmsDevice
    {
        public string DeviceId { get; set; }

        public string Alias { get; set; }

        public CommunicateType CommunicateType { get; set; }

        public EmsConnectionOption ConnectionOptions { get; set; }

        //public EmsPointMap PointMap { get; set; }

        public List<EmsDevicePointAlert> AlertPoints { get; set;  }

        public int OrderNumber { get; set; }

        public string EmsProtocol { get; set; }
    }
}
