using org.Models.Messagings;
using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Mqtt;

namespace org.Communication.Extensions
{
    public static class MqttExtensions
    {
        public static Models.Messagings.MqttMessage ToMessage(this MqttArrivedMessage arrivedMessage)
        {
            return new Models.Messagings.MqttMessage
            {
                QosLevel = (int)arrivedMessage.QosLevel,
                Retain = arrivedMessage.Retain,
                TopicName = arrivedMessage.TopicName,
                Payload = arrivedMessage.Payload
            };
        }
    }
}
