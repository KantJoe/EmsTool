using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.Utils;

namespace org.Communication.MqttTopics
{
    public class SubmitCollector04Topic : TopicBase
    {
        public SubmitCollector04Topic(string sn)
        {
            Topic = "0x04" + "/" + sn;
        }

        private void ComposeDataCore(ModbusDataStorage storage, List<byte> payload)
        {
            var emsSn = new byte[30];
            var group03_first = storage.Dequeue(storage.ModbusKey + "_3_0");
            var buff_emsSn = Encoding.ASCII.GetBytes(group03_first.GetStringFromBigEndian(59, 15)?.TrimEnd('0'));
            Array.Copy(buff_emsSn, emsSn, buff_emsSn.Length);
            payload.AddRange(emsSn);

            var index_emsSn = payload.Count;

            var now = DateTime.UtcNow;
            byte[] buff_nowTimeUtc = [
                (byte)(now.Year % 2000),(byte)(now.Month),(byte)(now.Day),
                (byte)(now.Hour),(byte)(now.Minute),(byte)(now.Second)];
            payload.AddRange(buff_nowTimeUtc);

            payload.Add((byte)storage.PatitionBuffer.Count);

            foreach (var buff in storage.PatitionBuffer
                .Where(w => w.Key.StartsWith(storage.ModbusKey + "_4_"))
                .OrderBy(o => o.Key.Length).ThenBy(t => t.Key).ToList())
            {
                var keys = buff.Key.Split("_");
                var start = ushort.Parse(keys.Last());
                var buffer = storage.Dequeue(buff.Key);

                var end = start + buffer.Length;
                payload.AddRange([(byte)(start >> 8), (byte)(start & 0xFF)]);
                payload.AddRange([(byte)(end >> 8), (byte)(end & 0xFF)]);
                payload.AddRange(buffer.ToArray().GetBytesFromBigEndian());
            }
        }

        public override void ComposeData(string modbusKey, params string[] patitionTopics)
        {
            CommunicateAdapterPool.StoragePool.TryGetValue(modbusKey, out ModbusDataStorage storage);

            var payload = new List<byte>();
            payload.AddRange([0x5a, 0xa5, 00, 00, 00, 0x04]);

            ComposeDataCore(storage, payload);

            var buff = payload.ToArray();
            var length = buff.Length - 3;
            byte[] buff_length = [(byte)(length >> 8), (byte)(length & 0xFF)];

            Array.Copy(buff_length, 0, buff, 3, 2);

            Buffer = buff.AsMemory();
        }

    }
}
