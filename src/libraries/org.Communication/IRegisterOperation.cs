using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace org.Communication
{
    public interface IRegisterOperation
    {
        Task WriteHoldingRegisters(byte slaveId, ushort startingAddress, ReadOnlyMemory<byte> bytes);
        Task<ReadOnlyMemory<ushort>> ReadHoldingRegisters(byte slaveId, ushort startingAddress, ushort count);
        Task<ReadOnlyMemory<ushort>> ReadInputRegisters(byte slaveId, ushort startingAddress, ushort count);
    }
}
