using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class LivingTracedPoint
    {
        public string Key { get; set; }

        public ModbusFunctionCode FunctionCode { get; set; }

        public ushort Address { get; set; }


    }
}
