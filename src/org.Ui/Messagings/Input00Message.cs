using org.Models.Messagings;
using org.Utils;
using ScottPlot.LegendLayouts;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace org.Ui.Messagings
{
    public partial class Input00Message : UshortMessage
    {
        public const string KEY = "4_0";

        public string EmsFirmwareVersion
        {
            get
            {
                var val = Buffer.GetStringFromBigEndian(33,10)?.TrimEnd('\0');
                return val;
            }
        }

        public Input00Message()
            : base(new ushort[125])
        {

        }
        public Input00Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }
        public Input00Message(ReadOnlyMemory<ushort> buffer,string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
