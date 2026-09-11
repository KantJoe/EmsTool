using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public  class Input2125Message:UshortMessage
    {
        public const string KEY = "4_2125";

        public Input2125Message()
            : base(new ushort[125])
        {

        }

        public Input2125Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Input2125Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
