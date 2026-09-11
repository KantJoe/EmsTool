using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public class PointViewModel
    {
        public int Address { get; set; }
        public string AddressStr => Address.ToString().PadLeft(4, '0');

        public ushort CurrentValue { get; set; }

        public string UpdateDateTime { get; set; }

        public string Key { get; set; }

        public int FunctionCode { get; set; } = 3;

        public int PointCount { get; set; } = 1;

        public string DateType { get; set; }

        public string NextValue { get; set; }
    }
}
