using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.Utils;

namespace org.Communication.MqttTopics
{
    public class SubmitCollector03Topic : TopicBase
    {
        public SubmitCollector03Topic(string sn)
        {
            Topic = "0x03" + "/" + sn;
        }

        private void ComposeDataCore(ModbusDataStorage storage, List<byte> payload)
        {
            var emsSn = new byte[30];
            var index_emsSn = payload.Count;

            var now = DateTime.UtcNow;
            byte[] buff_nowTimeUtc = [
                (byte)(now.Year % 2000),(byte)(now.Month),(byte)(now.Day),
                (byte)(now.Hour),(byte)(now.Minute),(byte)(now.Second)];
            payload.AddRange(buff_nowTimeUtc);

            payload.Add((byte)storage.PatitionBuffer.Count);

            foreach (var buff in storage.PatitionBuffer
                .Where(w => w.Key.StartsWith(storage.ModbusKey + "_3_"))
                .OrderBy(o => o.Key.Length).ThenBy(t => t.Key).ToList())
            {
                var keys = buff.Key.Split("_");
                var buffer = storage.Dequeue(buff.Key);

                var start = ushort.Parse(keys.Last());
                if (start == 0)
                {
                    var buff_emsSn = Encoding.ASCII.GetBytes(buffer.GetStringFromBigEndian(59, 15)?.TrimEnd('0'));
                    Array.Copy(buff_emsSn, emsSn, buff_emsSn.Length);
                    payload.InsertRange(index_emsSn, emsSn);
                }

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
            payload.AddRange([0x5a, 0xa5, 00, 00, 00, 0x03]);

            ComposeDataCore(storage, payload);

            var buff = payload.ToArray();
            var length = buff.Length - 3;
            byte[] buff_length = [(byte)(length >> 8), (byte)(length & 0xFF)];

            Array.Copy(buff_length, 0, buff, 3, 2);

            Buffer = buff.AsMemory();
        }
    }
}
