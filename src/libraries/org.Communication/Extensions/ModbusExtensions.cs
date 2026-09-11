using org.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Modbus;
using org.Utils;
using TouchSocket.Core;

namespace org.Communication.Extensions
{
    public static class ModbusExtensions
    {
        public static string GetKey(this EmsConnectionOption options)
        {
            return string.IsNullOrEmpty(options?.PortName) ? $"{options.Ip}_{options.Port}" : options.PortName;
        }

        private static async Task<IModbusResponse> ReadHoldingRegistersAsync(
            this IModbusMaster client, byte slaveId, ushort startingAddress, ushort length, int timeout = 1000)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                token.ThrowIfCancellationRequested();
                return await client.ReadHoldingRegistersAsync(
                    slaveId, startingAddress, length, timeout, token);
            }
            catch (TimeoutException toEx)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ReadHoldingRegisters Error: {toEx.Message},{startingAddress}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ReadHoldingRegisters Error: {ex.Message},{startingAddress}");
                throw;
            }

            return default;
        }
        public static async Task<ReadOnlyMemory<ushort>> ReadHoldingRegistersDataAsync(
            this IModbusMaster client, byte slaveId, ushort startingAddress, ushort length, int timeout = 1000)
        {
            var response = await client.ReadHoldingRegistersAsync(
                slaveId, startingAddress, length, timeout);
            if (response?.ErrorCode != ModbusErrorCode.Success)
            {
                return default;
            }

            return response?.Data.GetBigEndianU16s() ?? default;

        }

        private static async Task<IModbusResponse> ReadInputRegistersAsync(
            this IModbusMaster client, byte slaveId, ushort startingAddress, ushort length, int timeout = 1000)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                token.ThrowIfCancellationRequested();
                return await client.ReadInputRegistersAsync(
                        slaveId, startingAddress, length, timeout, token);
            }
            catch (TimeoutException toEx)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ReadInputRegisters Error: {toEx.Message},{startingAddress}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ReadInputRegisters Error: {ex.Message},{startingAddress}");
                throw;
            }

            return default;
        }

        public static async Task<ReadOnlyMemory<ushort>> ReadInputRegistersDataAsync(
            this IModbusMaster client, byte slaveId, ushort startingAddress, ushort length, int timeout = 1000)
        {
            var response = await client.ReadInputRegistersAsync(
                slaveId, startingAddress, length, timeout);
            if (response?.ErrorCode != ModbusErrorCode.Success)
            {
                return default;
            }

            return response?.Data.GetBigEndianU16s() ?? default;

        }

        public static async Task<byte[]> SendModbusRequestExtAsync(
            this IModbusObject client, ModbusRequestExt requestExt, int timeout = 5000)
        {
            IModbusMaster modbus = default;
            if (client is ModbusRtuObject rtu)
            {
                modbus = rtu.Instance;
            }
            else if (client is ModbusTcpObject tcp)
            {
                modbus = tcp.Instance;
            }

            var request = new ModbusRequest(client.Options.SlaveId, (FunctionCode)requestExt.FunctionCode, requestExt.StartingAddress, requestExt.Count);
            if (requestExt.Data.Length > 0)
            {
                request.Data = requestExt.Data;
            }
            var response = await modbus.SendModbusRequestExtAsync(request, timeout, CancellationToken.None);
            return response.Data.ToArray();
        }
    }
}
