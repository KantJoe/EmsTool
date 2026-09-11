using org.Models;
using org.Utils;
using org.Utils.Global;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Modbus;
using TouchSocket.Modbus.Adapter.Rtu;
using TouchSocket.SerialPorts;
using TouchSocket.Sockets;

namespace org.Communication
{
    public class ModbusRtuObject : IModbusObject
    {
        public EmsConnectionOption Options { get; private set; }
        public SerialPortOption InnerOptions { get; private set; }

        public ModbusRtuMaster Instance { get; private set; }
        public bool Online => Instance?.Online ?? false;
        public string Key => Options?.PortName ?? string.Empty;

        public async Task<bool> ConnectAsync()
        {
            await Instance?.ConnectAsync();

            return Instance?.Online ?? false;
        }

        public async Task DisconnectAsync()
        {
            using (var tokenSource = new CancellationTokenSource(2000))
            {
                var token = tokenSource.Token;
                token.ThrowIfCancellationRequested();
                await Instance?.CloseAsync().WithCancellation( token);
            }
        }

        public async Task InitializeAsync(EmsConnectionOption options)
        {
            Options = options;
            Instance = new ModbusRtuMaster();
            var config = new TouchSocketConfig();
            Instance.SetAdapter(new OrgModbusRtuAdapter());
            config.SetSerialPortOption(opt =>
            {
                opt.BaudRate = int.Parse(options.BaudRate);
                opt.DataBits = int.Parse(options.DataBits);
                opt.Parity = Enum.Parse<Parity>(options.Parity);
                opt.PortName = options.PortName;
                opt.StopBits = Enum.Parse<StopBits>(options.StopBits);
            });

            config.ConfigurePlugins(a =>
            {
                a.Add<ModbusIntervalPollingPlugin>();
            });

            await Instance.SetupAsync(config);

            InnerOptions = config.GetSerialPortOption();
        }

        public async Task CloseAsync()
        {
            try
            {
                await DisconnectAsync();
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "ModbusRtuObject.CloseAsync");
            }

            Instance?.Dispose();
        }

        public async Task WriteHoldingRegisters(byte slaveId, ushort startingAddress, ReadOnlyMemory<byte> bytes)
        {
            if (Instance?.Online != true)
            {
                return;
            }

            var response = await Instance.WriteMultipleRegistersAsync(slaveId, startingAddress, bytes, 5000, CancellationToken.None);

        }

        public async Task<ReadOnlyMemory<ushort>> ReadHoldingRegisters(byte slaveId, ushort startingAddress, ushort count)
        {
            if (Instance?.Online != true)
            {
                return Array.Empty<ushort>();
            }

            var response = await Instance.ReadHoldingRegistersAsync(slaveId, startingAddress, count, 5000, CancellationToken.None);
            if (response.ErrorCode != ModbusErrorCode.Success)
            {
                return default;
            }

            return response.Data.GetBigEndianU16s();
        }

        public async Task<ReadOnlyMemory<ushort>> ReadInputRegisters(byte slaveId, ushort startingAddress, ushort count)
        {
            if (Instance?.Online != true)
            {
                return Array.Empty<ushort>();
            }

            var response = await Instance.ReadInputRegistersAsync(slaveId, startingAddress, count);
            if (response.ErrorCode != ModbusErrorCode.Success)
            {
                return default;
            }

            return response.Data.GetBigEndianU16s();
        }
    }
}
