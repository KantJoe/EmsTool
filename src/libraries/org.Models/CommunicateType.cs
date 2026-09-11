using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{

    public enum CommunicateType
    {
        None,
        ModbusRtu,
        ModbusTcp,
        MqttClient,
        MqttClientOverWs
    }
}
