using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace org.Utils
{
    public static class ByteUtil
    {
        public static ReadOnlyMemory<ushort> GetBigEndianU16s(this ReadOnlyMemory<byte> buffer)
        {
            var length = buffer.Length;
            var buffers = new ushort[length / 2];
            for (int index = 0; index < buffers.Length; index++)
            {
                buffers[index] = buffer.GetPoint(index * 2);
            }

            return buffers;
        }

        public static ushort GetPoint(this ReadOnlyMemory<byte> buff, int start = 0)
        {
            return buff.Span.GetBigEndianU16(start);
        }

        public static ushort GetBigEndianU16(this ReadOnlySpan<byte> buffer, int start = 0)
        {
            if (buffer.Length > (start + 1))
            {
                var array = buffer.Slice(start, 2);

                return (ushort)((array[0] << 8) + array[1]);
            }

            return (ushort)0;
        }

        public static ReadOnlyMemory<ushort> GetBigEndianU16s(this ReadOnlySpan<byte> buffer)
        {
            var length = buffer.Length;
            var buffers = new ushort[length / 2];
            for (int index = 0; index < buffers.Length; index++)
            {
                buffers[index] = buffer.GetBigEndianU16(index * 2);
            }

            return buffers;
        }

        public static string GetStringFromBigEndian(this ReadOnlyMemory<ushort> buff, int start, int count)
        {
            if (buff.Length < start)
            {
                return string.Empty;
            }

            var array = buff.Length >= (start + count + 1) ? buff.Slice(start, count) : buff.Slice(start);
            var buffer = array.ToArray().GetBytesFromBigEndian();

            return System.Text.Encoding.ASCII.GetString(buffer);
        }

        public static byte[] GetBytesFromBigEndian(this ushort[] buff)
        {
            if (buff?.Any() != true) { return Array.Empty<byte>(); }

            byte[] byteArray = new byte[buff.Length * 2]; // 创建一个足够大的byte数组

            for (int i = 0; i < buff.Length; i++)
            {
                var single = buff[i];
                byteArray[i * 2] = (byte)(single >> 8); // 高字节
                byteArray[i * 2 + 1] = (byte)(single & 0xFF); // 低字节
            }

            return byteArray;
        }

        public static ushort GetPoint(this ushort[] buff, int start = 0)
        {
            return (ushort)(buff.Length > start ? buff[start] : 0);
        }

        public static ushort GetPoint(this ReadOnlyMemory<ushort> buffer, int start = 0)
        {
            return (ushort)(buffer.Length > start ? buffer.Span[start] : 0);
        }

        public static UInt32 GetPointU32(this ReadOnlyMemory<ushort> buffer, int start)
        {
            if (buffer.Length < (start + 2))
            {
                return 0u;
            }

            var ushorts = buffer.Slice(start, 2).ToArray();
            return (UInt32)((ushorts[0] << 16) + ushorts[1]);
        }
        public static UInt32 GetPointU32(this ushort[] buffer, int start)
        {
            if (buffer.Length < (start + 2))
            {
                return 0u;
            }

            var ushorts = buffer.Skip(start).Take(2).ToArray();
            return (UInt32)((ushorts[0] << 16) + ushorts[1]);
        }

        public static Int32 GetPoint32(this ReadOnlyMemory<ushort> buffer, int start)
        {
            if (buffer.Length < (start + 2))
            {
                return 0;
            }

            var ushorts = buffer.Slice(start, 2).ToArray();
            return ((ushorts[0] << 16) + ushorts[1]);
        }

        public static ushort[] ToUInt16s(this decimal data)
        {
            var d = Convert.ToUInt32(data);
            return [(ushort)(d >> 16), (ushort)(d & 0xFFFF)];
        }

        public static ushort ToUInt16(this decimal data)
        {
            var d = Convert.ToUInt32(data);
            return (ushort)(d & 0xFFFF);
        }

        public static ushort SetBit(this ushort data, bool val, int bit)
        {
            if (bit < 0 || bit > 15)
            {
                return data;
            }

            return (ushort)(val ? (data | (1 << bit)) : (data & ~(1 << bit)));
        }
    }
}
