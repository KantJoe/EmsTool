using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class Holding00Message: UshortMessage
    {
        public const string KEY = "3_0";

        public Holding00Message(ReadOnlyMemory<ushort> buffer)
            :base(buffer)
        {
        }
    }
}
