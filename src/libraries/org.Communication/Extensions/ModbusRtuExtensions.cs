using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Modbus;

namespace org.Communication.Extensions
{
    public static class ModbusRtuExtensions
    {
        public static async Task<IModbusResponse> EntryBurnFirmwareMode(
            this IModbusMaster master, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(
                slaveId, (FunctionCode)0x26, 0x0001, 1);
            return await master.SendModbusRequestExtAsync(
                request, timeout, token);
        }

        public static Task<IModbusResponse> ExitBurnFirmwareMode(
            this IModbusMaster master, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(
                slaveId, FunctionCode.ReadWriteMultipleRegisters, 0x0006, 2)
            {
                Data = new byte[] {
                    (byte)0,
                    (byte)0,
                }.AsMemory()
            };

            return master.SendModbusRequestExtAsync(
                request, timeout, token);
        }

        public static Task<IModbusResponse> GetBurnFirmwareProgress(
            this IModbusMaster master, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(slaveId, (FunctionCode)0x23, 0x0001, 2)
            {
                Data = new byte[2] { 00, 01 }.AsMemory()
            };
            return master.SendModbusRequestExtAsync(
                request, timeout, token);
        }

        public static Task<IModbusResponse> DownloadFirmware(
            this IModbusMaster master, ushort buffLength, byte[] buff, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(slaveId, FunctionCode.ReadWriteMultipleRegisters, 0x0005, buffLength)
            {
                Data = buff.AsMemory()
            };
            return master.SendModbusRequestExtAsync(
                request, timeout, token);
        }

        public static Task<IModbusResponse> ClearFirmwareFlash(
            this IModbusMaster master, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(slaveId, FunctionCode.ReadWriteMultipleRegisters, 0x0004, 2)
            {
                Data = new byte[2].AsMemory()
            };
            return master.SendModbusRequestExtAsync(
                request, timeout, token);
        }

        public static Task ValidateFirmwareCRC32(
            this IModbusMaster master, uint crc32, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(slaveId, FunctionCode.ReadWriteMultipleRegisters, 0x0003, 4)
            {
                Data = new byte[] {
                    (byte)(crc32>>24&0xff),
                    (byte)(crc32>>16&0xff),
                    (byte)(crc32>>8&0xff),
                    (byte)(crc32&0xff)}.AsMemory()
            };
            return master.SendModbusRequestExtAsync(
                request, timeout, token);
        }

        public static async Task<IModbusResponse> WriteFirmwareSize(
            this IModbusMaster master, int size, byte slaveId = 1, int timeout = 5000, CancellationToken token = default)
        {
            var request = new ModbusRequest(slaveId, FunctionCode.ReadWriteMultipleRegisters, 0x0002, 4)
            {
                Data = new byte[] {
                (byte)(size>>24 & 0xFF),
                (byte)(size>>16 & 0xFF),
                (byte)(size>>8 & 0xFF),
                (byte)(size>>0 & 0xFF)}.AsMemory()
            };

            return await master.SendModbusRequestExtAsync(
                request, timeout, token);
        }
    }
}
