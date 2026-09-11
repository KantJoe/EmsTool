using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Messagings
{
    public class RawMqttMessage
    {
        /// <summary>
        /// 3保持寄存器
        /// 4输入寄存器
        /// </summary>
        public int FunctionCode { get; set; }

        public int StartAddress { get; set; }

        public int EndAddress { get; set; }

        public string Topic { get; set; }

        public int QosLevel { get; set; }

        public string Sn { get; set; }
        /// <summary>
        /// key: ip_port_functioncode_start_end
        /// value:
        /// 0x04/sn:        head:42...start:2,end:2,data:(end-start+1)*2
        /// 0x05/result/sn: head:36...start:2,end:2,data:(end-start+1)*2
        /// 0x06/result/sn: head:36...start:2,start:2,code:1,reason:1
        /// 0x10/result/sn: head:36...start:2,end:2,code:1,reason:1
        /// </summary>
        public List<KeyValuePair<string, ReadOnlyMemory<byte>>> Buffers { get; set; }
    }
}
