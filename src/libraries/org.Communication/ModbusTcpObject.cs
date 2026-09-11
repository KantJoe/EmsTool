using org.Models;
using org.Utils;
using org.Utils.Global;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Modbus;
using TouchSocket.Sockets;

namespace org.Communication
{
    public class ModbusTcpObject : IModbusObject
    {
        public EmsConnectionOption Options { get; private set; }
        public IPHost InnerOptions { get; private set; }
        public ModbusTcpMaster Instance { get; private set; }
        public bool Online => Instance?.Online ?? false;
        public string Key => $"{Options?.Ip}_{Options?.Port}";

        public async Task<bool> ConnectAsync()
        {
            try
            {
                await Instance?.ConnectAsync();
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"{Options.Ip}:{Options.Port}");
            }

            return Instance?.Online ?? false;
        }

        public async Task DisconnectAsync()
        {
            await Instance?.CloseAsync();
        }

        public async Task InitializeAsync(EmsConnectionOption options)
        {
            Options = options;
            Instance = new ModbusTcpMaster();
            var config = new TouchSocketConfig();
            config.SetRemoteIPHost($"{options.Ip}:{options.Port}");
            config.ConfigurePlugins(a =>
            {
                a.UseReconnection<ModbusTcpMaster>(options =>
                {
                    options.PollingInterval = TimeSpan.FromSeconds(10);
                });

                a.Add<ModbusIntervalPollingPlugin>();
            });

            await Instance.SetupAsync(config);
            InnerOptions = config.GetRemoteIPHost();
        }

        public async Task CloseAsync()
        {
            await Instance?.CloseAsync();
            Instance?.Dispose();
        }

        public async Task WriteHoldingRegisters(byte slaveId, ushort startingAddress, ReadOnlyMemory<byte> bytes)
        {
            if (Instance?.Online != true)
            {
                return;
            }

            await Instance.WriteMultipleRegistersAsync(slaveId, startingAddress, bytes);
        }

        public async Task<ReadOnlyMemory<ushort>> ReadHoldingRegisters(byte slaveId, ushort startingAddress, ushort count)
        {
            if (Instance?.Online != true)
            {
                return Array.Empty<ushort>();
            }

            var response = await Instance.ReadHoldingRegistersAsync(slaveId, startingAddress, count);
            return response.Data.GetBigEndianU16s();
        }

        public async Task<ReadOnlyMemory<ushort>> ReadInputRegisters(byte slaveId, ushort startingAddress, ushort count)
        {
            if (Instance?.Online != true)
            {
                return Array.Empty<ushort>();
            }

            var response = await Instance.ReadInputRegistersAsync(slaveId, startingAddress, count);
            return response.Data.GetBigEndianU16s();
        }
    }
}
