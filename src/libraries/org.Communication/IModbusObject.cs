using org.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Modbus;

namespace org.Communication
{
    public interface IModbusObject:IConnectClient, IRegisterOperation
    {
    }
}
