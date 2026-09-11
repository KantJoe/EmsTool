using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.Communication.MqttTopics
{
    public class MqttStateResultMessage : MqttResultMessage
    {
        /// <summary>
        /// 状态码
        /// 0: 成功
        /// 1：解析错误
        /// 2：数据异常
        /// </summary>
        public byte State => Buff.Slice(6, 1).ToArray().FirstOrDefault();

        public MqttStateResultMessage(ReadOnlySequence<byte> sequence) : base(sequence)
        {
        }
    }
}
