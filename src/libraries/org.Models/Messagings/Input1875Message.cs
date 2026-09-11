using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class Input1875Message:UshortMessage
    {
        public const string KEY = "4_1875";

        public Input1875Message()
            : base(new ushort[125])
        {

        }

        public Input1875Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Input1875Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
