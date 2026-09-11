using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class EmsSolution
    {
        public string EmsSolutionName { get; set; }
        public List<EmsDevice> DeviceTopologies { get; set; }

        public EmsSolutionDocument EmsSolutionDocument { get; set; }

        public EmsSettings Settings { get; set; }

        public List<string> DynamicModules { get; set; }
    }
}
