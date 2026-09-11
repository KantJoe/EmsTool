using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class Input0250Message:UshortMessage
    {
        public const string KEY = "4_250";

        public Input0250Message()
            : base(new ushort[125])
        {

        }

        public Input0250Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Input0250Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
