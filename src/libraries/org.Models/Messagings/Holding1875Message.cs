using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class Holding1875Message:UshortMessage
    {
        public const string KEY = "3_1875";

        public Holding1875Message()
            : base(new ushort[125])
        {

        }

        public Holding1875Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Holding1875Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
