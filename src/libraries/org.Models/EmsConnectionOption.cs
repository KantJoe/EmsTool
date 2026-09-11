using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class EmsConnectionOption
    {
        #region modbus rtu
        public string PortName { get; set; }
        public string DataBits { get; set; }
        public string BaudRate { get; set; }
        public string Parity { get; set; }
        public string StopBits { get; set; }
        public byte SlaveId { get; set; }
        #endregion

        #region modbus tcp
        public string Ip { get; set; }
        public string Port { get; set; }
        #endregion

        #region mqtt
        /// <summary>
        /// 协议号 如: mqtt:// tcp:// ws://
        /// </summary>
        public string Protocol { get; set; }
        #endregion

        public int OrderNumber { get; set; }

        public static EmsConnectionOption CreateDefaultRtuOptions(int orderNumber = 1,byte slaveId=1)
        {
            return new EmsConnectionOption
            {
                BaudRate = "115200",
                DataBits = "8",
                PortName = "COM99",
                Parity = "0",
                StopBits = "1",
                SlaveId = slaveId,
                OrderNumber = orderNumber
            };
        }

        public static EmsConnectionOption CreateDefaultTcpOptions(int orderNumber = 1)
        {
            return new EmsConnectionOption
            {
                Ip = "127.0.0.1",
                Port = "502",
                OrderNumber = orderNumber
            };
        }

        public static EmsConnectionOption CreateDefaultMqttClientOptions(int orderNumber = 1)
        {
            return new MqttEnvironmentConfig
            {
                Protocol = "mqtt://",
                Ip = "127.0.0.1",
                Port = "1883",
                ClientId = Guid.NewGuid().ToString("N"),
                MqttVersion = 5,
                OrderNumber = orderNumber
            };
        }
    }
}
