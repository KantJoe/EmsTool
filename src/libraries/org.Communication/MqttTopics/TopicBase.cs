using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Mqtt;

namespace org.Communication.MqttTopics
{
    public abstract class TopicBase
    {
        public string Topic { get; protected set; }

        public bool Retain { get; protected set; }

        public QosLevel QosLevel { get; protected set; }

        public ReadOnlyMemory<byte> Buffer { get; protected set; } = new ReadOnlyMemory<byte>();

        public abstract void ComposeData(string modbusKey, params string[] patitionTopics);

        public MqttPublishMessage ToMessage()
        {
            return new MqttPublishMessage(Topic, Retain, QosLevel, Buffer);
        }

        public async Task PublishAsync(string modbusKey,IMqttClient client)
        {
            if (string.IsNullOrEmpty(Topic) || Buffer.IsEmpty)
            {
                return;
            }

            await client.PublishAsync(ToMessage());
        }
    }
}
