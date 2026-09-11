using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.Communication.MqttTopics
{
    public class MqttResultMessage
    {
        public ReadOnlyMemory<byte> Buff { get; private set; }

        public ushort Length  => BinaryPrimitives.ReadUInt16BigEndian(Buff.Slice(3, 2).Span); 

        public byte Code => Buff.Slice(5,1).ToArray().FirstOrDefault();

        

        public MqttResultMessage(ReadOnlySequence<byte> sequence)
        {
            // 计算总长度
            var totalLength = sequence.Length;
            var memory = new byte[totalLength];
            sequence.CopyTo(memory); // 将序列复制到内存数组中
            Buff = memory.AsMemory();
        }
    }
}
