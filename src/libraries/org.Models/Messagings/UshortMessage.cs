using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class UshortMessage
    {
        public string Mapping { get; protected set; }
        public ReadOnlyMemory<ushort> Buffer { get; protected set; }

        public UshortMessage(ReadOnlyMemory<ushort> buff)
        {
            Buffer = buff;
        }

        public UshortMessage(ReadOnlyMemory<ushort> buff,string mapping)
        {
            Mapping = mapping;
            Buffer = buff;
        }
    }
}
