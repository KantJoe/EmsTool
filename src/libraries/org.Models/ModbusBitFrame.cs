using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public ref struct ModbusBitFrame
    {
        public byte SlaveIndex => Buffer[0];

        public byte FunctionCode => Buffer[1];

        public byte ByteLength => Buffer[2];

        public ReadOnlySpan<byte> Buffer { get; private set; }

        public ushort Crc16 => BinaryPrimitives.ReadUInt16LittleEndian(Buffer.Slice(Buffer.Length - 2));
        private const int BUFFER_START_INDEX = 3;

        //public bool GetData(int offset)
        //{
        //    return BinaryPrimitives.ReadUInt16LittleEndian(Buffer.Slice(BUFFER_START_INDEX + offset * 2, 2));
        //}

        //public ushort[] GetDatas(int ushortOffset, int ushortCount)
        //{
        //    const int times = 2;
        //    var buffer = new ushort[ushortCount];
        //    for (var index = 0; index < buffer.Length; index++)
        //    {
        //        buffer[index] = BinaryPrimitives.ReadUInt16LittleEndian(Buffer.Slice(BUFFER_START_INDEX + ushortOffset * times + index * times, times));
        //    }

        //    return buffer;
        //}

        public static ModbusBitFrame FromBytes(Span<byte> buffer)
        {
            return new ModbusBitFrame
            {
                Buffer = buffer.ToArray()
            };
        }
    }
}
