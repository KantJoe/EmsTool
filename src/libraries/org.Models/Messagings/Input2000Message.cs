using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class Input2000Message:UshortMessage
    {
        public const string KEY = "4_2000";

        public Input2000Message()
            : base(new ushort[125])
        {

        }

        public Input2000Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Input2000Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
