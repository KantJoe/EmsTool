using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class MqttMessage
    {
        /// <summary>
        /// 获取消息的服务质量级别。
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// 获取消息的有效负载。
        /// </summary>
        public ReadOnlySequence<byte> Payload { get; set; }

        /// <summary>
        /// 获取一个值，该值指示消息是否被保留。
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// 获取消息的主题名称。
        /// </summary>
        public string TopicName { get;set; }
    }
}
