using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class Holding0125Message : UshortMessage
    {
        public const string KEY = "3_125";

        public Holding0125Message()
            : base(new ushort[125])
        {

        }

        public Holding0125Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Holding0125Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
