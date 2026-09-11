using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class ModbusRequestExt
    {
        /// <inheritdoc/>
        public ReadOnlyMemory<byte> Data { get; set; }


        /// <inheritdoc/>
        public byte FunctionCode { get; set; }


        /// <inheritdoc/>
        public ushort Count { get; set; }


        /// <inheritdoc/>
        public ushort ReadCount { get; set; }


        /// <inheritdoc/>
        public ushort ReadStartAddress { get; set; }


        /// <inheritdoc/>
        public byte SlaveId { get; set; }


        /// <inheritdoc/>
        public ushort StartingAddress { get; set; }

    }
}
