using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Modbus;

namespace org.Communication
{
    public class OrgModbusRtuAdapter : CustomDataHandlingAdapter<ModbusRtuResponse>
    {
        protected override FilterResult Filter<TReader>(ref TReader reader, bool beCached, ref ModbusRtuResponse request)
        {
            if (reader.BytesRemaining < 3)
            {
                return FilterResult.Cache;
            }

            var pos = reader.BytesRead;

            var slaveId = ReaderExtension.ReadValue<TReader, byte>(ref reader);
            FunctionCode functionCode;
            var isError = false;
            var code = ReaderExtension.ReadValue<TReader, byte>(ref reader);
            if ((code & 0x80) == 0)
            {
                functionCode = (FunctionCode)code;
            }
            else
            {
                code = code.SetBit(7, false);
                functionCode = (FunctionCode)code;
                isError = true;
            }

            ModbusErrorCode errorCode;
            byte[] data;
            ushort startingAddress;

            int bodyLength;
            if (isError)
            {
                errorCode = (ModbusErrorCode)ReaderExtension.ReadValue<TReader, byte>(ref reader);
                bodyLength = 2;

                if (reader.BytesRemaining < bodyLength)
                {
                    reader.BytesRead = pos;
                    return FilterResult.Cache;
                }

                var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                //下面crc验证失败时，不再抛出错误，而是返回错误码。
                //https://gitee.com/RRQM_Home/TouchSocket/issues/IBC1J2

                if (crc == newCrc)
                {
                    request = new ModbusRtuResponse()
                    {
                        SlaveId = slaveId,
                        ErrorCode = errorCode,
                        FunctionCode = functionCode,
                    };

                    return FilterResult.Success;
                }
                else
                {
                    request = new ModbusRtuResponse()
                    {
                        SlaveId = slaveId,
                        ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                        FunctionCode = functionCode,
                    };

                    return FilterResult.Success;
                }
            }
            else
            {
                if ((byte)functionCode <= 4)
                {
                    bodyLength = ReaderExtension.ReadValue<TReader, byte>(ref reader) + 2;

                    if (reader.BytesRemaining < bodyLength)
                    {
                        reader.BytesRead = pos;
                        return FilterResult.Cache;
                    }

                    var len = bodyLength - 2;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);

                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                }
                else if ((int)functionCode == 0x08)
                {
                    bodyLength = (int)reader.BytesRemaining;
                    var len = bodyLength - 2;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);
                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            Data = data,
                            Crc = crc
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                }
                else if (functionCode == FunctionCode.ReadWriteMultipleRegisters)
                {
                    var subCode = ReaderExtension.ReadValue<TReader, byte>(ref reader);
                    bodyLength = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big) + 2;

                    if (reader.BytesRemaining < bodyLength)
                    {
                        reader.BytesRead = pos;
                        return FilterResult.Cache;
                    }

                    var len = bodyLength - 2;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);

                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                }
                else if (functionCode == FunctionCode.WriteSingleCoil || functionCode == FunctionCode.WriteSingleRegister)
                {
                    bodyLength = 6;
                    if (reader.BytesRemaining < bodyLength)
                    {
                        reader.BytesRead = pos;
                        return FilterResult.Cache;
                    }
                    startingAddress = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    var len = bodyLength - 4;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);
                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            StartingAddress = startingAddress,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            StartingAddress = startingAddress,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }

                    //this.m_headerLength = byteBlock.Position - pos;
                    //return true;
                }
                else if (functionCode == FunctionCode.WriteMultipleCoils || functionCode == FunctionCode.WriteMultipleRegisters)
                {
                    bodyLength = 6;
                    if (reader.BytesRemaining < bodyLength)
                    {
                        reader.BytesRead = pos;
                        return FilterResult.Cache;
                    }
                    startingAddress = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);
                    var quantity = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);
                    data = new byte[0];

                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            StartingAddress = startingAddress,
                            Quantity = quantity,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            StartingAddress = startingAddress,
                            Quantity = quantity,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                }
                else if ((int)functionCode == 0x23)
                {
                    //throw new System.Exception("无法识别的功能码");

                    bodyLength = ReaderExtension.ReadValue<TReader, byte>(ref reader) + 2;

                    if (reader.BytesRemaining < bodyLength)
                    {
                        reader.BytesRead = pos;
                        return FilterResult.Cache;
                    }

                    var len = bodyLength - 2;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);

                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            Data = data,
                            Crc = crc
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                }
                else if ((int)functionCode == 0x26)
                {
                    //throw new System.Exception("无法识别的功能码");

                    bodyLength = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big) * 2 + 2;

                    if (reader.BytesRemaining < bodyLength)
                    {
                        reader.BytesRead = pos;
                        return FilterResult.Cache;
                    }

                    var len = bodyLength - 2;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);

                    var newCrc = TouchSocketModbusUtility.ToModbusCrcValue(reader.TotalSequence.Slice(pos, reader.BytesRead - pos));
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    if (crc == newCrc)
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            Data = data,
                            Crc = crc
                        };
                        return FilterResult.Success;
                    }
                    else
                    {
                        request = new ModbusRtuResponse()
                        {
                            SlaveId = slaveId,
                            FunctionCode = functionCode,
                            ErrorCode = ModbusErrorCode.ResponseMemoryVerificationError,
                            Data = data,
                        };
                        return FilterResult.Success;
                    }
                }
                else
                {
                    bodyLength = (int)reader.BytesRemaining;
                    var len = bodyLength - 2;
                    data = reader.GetSpan(len).Slice(0, len).ToArray();
                    reader.Advance(len);
                    var crc = ReaderExtension.ReadValue<TReader, ushort>(ref reader, EndianType.Big);

                    request = new ModbusRtuResponse()
                    {
                        SlaveId = slaveId,
                        FunctionCode = functionCode,
                        Data = data,
                        ErrorCode = ModbusErrorCode.ValueInvalid,
                        Crc = crc
                    };

                    return FilterResult.Success;
                }
            }
        }
    }
}
