using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchSocket.Core;
using org.Utils;
using org.Utils.Global;

namespace org.Communication.MqttTopics
{
    public class SubmitCollector02Topic : TopicBase
    {
        public SubmitCollector02Topic()
        {
            Topic = "0x02";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffers">03第一组、03第四组</param>
        public override void ComposeData(string modbusKey, params string[] patitionTopics)
        {
            CommunicateAdapterPool.StoragePool.TryGetValue(modbusKey, out ModbusDataStorage storage);
            var group03_First = storage.Dequeue(patitionTopics.First());
            var group03_Fourth = storage.Dequeue(patitionTopics.Last());

            var payload = new List<byte>();
            payload.AddRange([0x5a, 0xa5, 00, 00, 00, 0x02]);

            var sn = group03_First.Slice(74, 8).ToArray();
            var buff_Sn = new byte[16];
            Array.Copy(sn.GetBytesFromBigEndian(), buff_Sn, 16);
            payload.AddRange(buff_Sn);

            var collector = ConfigContext.GenericConfig?.MqttConfig?.CollectorModel ?? string.Empty;
            var buff_collector = new byte[20];
            var tmpBuff = Encoding.ASCII.GetBytes(collector);
            Array.Copy(tmpBuff, buff_collector, tmpBuff.Length);
            payload.AddRange(buff_collector);

            byte buff_state = 1;
            payload.Add(buff_state);

            var buff_linkState = (byte)group03_Fourth.Slice(0, 1).ToArray().Single();
            payload.Add(buff_linkState);

            byte buff_strength = 99;
            payload.Add(buff_strength);

            var brand = ConfigContext.GenericConfig?.MqttConfig?.CollectorBrand ?? string.Empty;
            var buff_brand = new byte[10];
            tmpBuff = Encoding.ASCII.GetBytes(brand);
            Array.Copy(tmpBuff, buff_brand, tmpBuff.Length);
            payload.AddRange(buff_brand);

            var version = ConfigContext.GenericConfig?.MqttConfig?.CollectorVersion ?? string.Empty;
            var buff_version = new byte[20];
            tmpBuff = Encoding.ASCII.GetBytes(version);
            Array.Copy(tmpBuff, buff_version, tmpBuff.Length);
            payload.AddRange(buff_version);

            var ip = group03_Fourth.Slice(11, 4).ToArray();
            payload.AddRange(ip.Select(s => (byte)s));

            byte deviceCount = 1;
            payload.Add(deviceCount);

            var deviceSn = group03_First.Slice(59, 15).ToArray();
            var buff_deviceSn = new byte[30];
            Array.Copy(deviceSn.GetBytesFromBigEndian(), buff_deviceSn, 30);
            payload.AddRange(buff_deviceSn);

            var buff = payload.ToArray();
            var length = buff.Length - 3;
            byte[] buff_length = [(byte)(length >> 8), (byte)(length & 0xFF)];

            Array.Copy(buff_length, 0, buff, 3, 2);

            Buffer = buff.AsMemory();
        }
    }
}
