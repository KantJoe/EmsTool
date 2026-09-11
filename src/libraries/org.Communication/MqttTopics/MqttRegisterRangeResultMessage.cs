using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.Utils;

namespace org.Communication.MqttTopics
{
    public class MqttRegisterRangeResultMessage : MqttResultMessage
    {
        public ushort Start => Buff.GetPoint(6);

        public MqttRegisterRangeResultMessage(ReadOnlySequence<byte> sequence) : base(sequence)
        {
        }
    }
}
