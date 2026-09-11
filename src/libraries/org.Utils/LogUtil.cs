using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace org.Utils
{
    public static class LogUtil
    {
        public static string ToLogString(this ReadOnlyMemory<ushort> buffer)
        {
            if (buffer.IsEmpty)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in buffer.Span)
            {
                sb.Append("," + item);
            }

            return sb.ToString();
        }

        public static string ToLogString(this ReadOnlySequence<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                return string.Empty;
            }

            // 计算总长度
            var totalLength = buffer.Length;
            var memory = new byte[totalLength];
            return ToLogString(memory);
        }

        public static string ToLogString(this byte[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            for (var index = 0; index < buffer.Length; index += 2)
            {
                sb.Append($" {buffer[index]:x2}" +
                    $"{(buffer.Length > (index + 1) ? buffer[index + 1] : ""):x2}");
                if (index >= 62 && ((index + 2) % 64 == 0))
                {
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
    }
}
