using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace org.Models
{
    public class EmsPoint
    {
        [ExcelColumn("功能码/FunctionCode")]
        public ModbusFunctionCode FunctionCode { get; set; }

        [ExcelColumn("分组/Group")]
        public string GroupName { get; set; }

        [ExcelColumn("地址/Address")]
        public uint Address { get; set; }

        [ExcelColumn("描述/Description")]
        public string Description { get; set; }

        public string DataType { get; set; }

        public Type ValueType { get; set; }

        [ExcelColumn("位长/BitsLength")]
        public int BitsLength { get; set; }

        [ExcelColumn("读写类型/Kind")]
        public ReadWriteKind Kind { get; set; }

        /// <summary>
        ///  大端字节序
        /// </summary>
        [ExcelColumn("值/Value")]
        [JsonIgnore]
        public byte[] Value { get; set; }

        /// <summary>
        /// 内部系数关联值可直接获得，外部关联公式需要自行计算
        /// </summary>
        [JsonIgnore]
        public object ActualValue
        {
            get => GetActualValue();
            set
            {
                Value = SetActualValue(value);
            }
        }

        [ExcelColumn("默认值/DefaultValue")]
        public byte[] DefaultValue { get; set; }

        [ExcelColumn("系数/Coefficient")]
        public decimal Coefficient { get; set; }

        [ExcelColumn("单位/Unit")]
        public string Unit { get; set; }

        [ExcelColumn("值范围/ValueScope")]
        public string ValueScope { get; set; }

        [ExcelColumn("最小值（包含）/MinValue")]
        public byte[] MinValue { get; set; }

        [ExcelColumn("最大值（包含）/MaxValue")]
        public byte[] MaxValue { get; set; }

        public static ReadWriteKind KindConvertFromStr(string kind)
        {
            switch (kind?.ToLower())
            {
                case "r":
                    return ReadWriteKind.Read;
                case "w":
                    return ReadWriteKind.Write;
                case "r/w":
                case "w/r":
                    return ReadWriteKind.ReadAndWrite;
                default:
                    return ReadWriteKind.None;
            }
        }

        public byte[] SetActualValue(object val)
        {
            switch (ValueType.FullName)
            {
                case "System.UInt16":
                    if (!ushort.TryParse(val.ToString(), out ushort us))
                    {
                        return new byte[2];
                    }

                    return BitConverter.GetBytes(us);
                case "System.Int16":
                    if (!short.TryParse(val.ToString(), out short s))
                    {
                        return new byte[2];
                    }

                    return BitConverter.GetBytes(s);
                case "System.Int32":
                    if (!Int32.TryParse(val.ToString(), out int i))
                    {
                        return new byte[4];
                    }

                    return BitConverter.GetBytes(i);
                case "System.UInt32":
                    if (!UInt32.TryParse(val.ToString(), out UInt32 ui))
                    {
                        return new byte[4];
                    }

                    return BitConverter.GetBytes(ui);
                case "System.Int64":
                    if (!long.TryParse(val.ToString(), out long l))
                    {
                        return new byte[8];
                    }

                    return BitConverter.GetBytes(l);
                case "System.UInt64":
                    if (!ulong.TryParse(val.ToString(), out ulong ul))
                    {
                        return new byte[8];
                    }

                    return BitConverter.GetBytes(ul);
                case "System.String":
                    return System.Text.Encoding.UTF8.GetBytes(val?.ToString()) ?? new byte[BitsLength / 8];
                default:
                    return new byte[0];
            }
        }

        /// <summary>
        /// 实际值，需要额外计算系数
        /// </summary>
        /// <returns></returns>
        public object GetActualValue()
        {
            var tmpBuff = new byte[0];
            var buff = Value?.AsEnumerable()?.Reverse()?.ToArray() ?? new byte[0];
            switch (ValueType.FullName)
            {
                case "System.UInt16":
                    tmpBuff = new byte[2];
                    Array.Copy(buff, tmpBuff, Math.Max(2, buff.Length));
                    return (ushort)(BitConverter.ToUInt16(tmpBuff) * Coefficient);
                case "System.Int16":
                    tmpBuff = new byte[2];
                    Array.Copy(buff, tmpBuff, Math.Max(2, buff.Length));
                    return (short)(BitConverter.ToInt16(buff) * Coefficient);
                case "System.Int32":
                    tmpBuff = new byte[4];
                    Array.Copy(buff, tmpBuff, Math.Max(4, buff.Length));
                    return (Int32)(BitConverter.ToInt32(buff) * Coefficient);
                case "System.UInt32":
                    tmpBuff = new byte[4];
                    Array.Copy(buff, tmpBuff, Math.Max(4, buff.Length));
                    return (UInt32)(BitConverter.ToUInt32(buff) * Coefficient);
                case "System.Int64":
                    tmpBuff = new byte[8];
                    Array.Copy(buff, tmpBuff, Math.Max(8, buff.Length));
                    return (long)(BitConverter.ToInt64(buff) * Coefficient);
                case "System.UInt64":
                    tmpBuff = new byte[8];
                    Array.Copy(buff, tmpBuff, Math.Max(8, buff.Length));
                    return (ulong)(BitConverter.ToUInt64(buff) * Coefficient);
                case "System.String":
                    return System.Text.Encoding.UTF8.GetString(buff);
                default:
                    return null;
            }
        }
    }

    public enum ReadWriteKind
    {
        None,
        Read = 1,
        Write = 2,
        ReadAndWrite = 3
    }

}
