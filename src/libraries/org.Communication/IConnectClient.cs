using org.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace org.Communication
{
    public interface IConnectClient
    {
        bool Online { get; }

        string Key { get; }
        EmsConnectionOption Options { get; }
        Task InitializeAsync(EmsConnectionOption options);
        Task<bool> ConnectAsync();
        Task DisconnectAsync();
        Task CloseAsync();
    }
}
