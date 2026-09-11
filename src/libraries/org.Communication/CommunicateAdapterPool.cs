using org.Communication.Extensions;
using org.Models;
using org.Ui;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace org.Communication
{
    public static class CommunicateAdapterPool
    {
        /// <summary>
        /// modbus 主站
        /// key：portname/ip+_+port
        /// </summary>
        public static Dictionary<string, IConnectClient> ModbusMasterPool { get; private set; }
        /// <summary>
        /// modbus数据缓存
        /// key：portname/ip+_+port
        /// </summary>
        public static Dictionary<string, ModbusDataStorage> StoragePool { get; private set; }
        /// <summary>
        /// mqttclient
        /// key：sn
        /// </summary>
        public static Dictionary<string, MqttClientObject> MqttClientPool { get; private set; }

        public static bool Online => ModbusMasterPool?.Values?.Any(a => a.Online) ?? false;


        static CommunicateAdapterPool()
        {
            ModbusMasterPool = new Dictionary<string, IConnectClient>();
            StoragePool = new Dictionary<string, ModbusDataStorage>();
            MqttClientPool = new Dictionary<string, MqttClientObject>();
        }

        #region modbus
        public static async Task InitializeModbusMastersAsync()
        {
            var devices = EmsSolutionContext.Current?.DeviceTopologies;
            if (devices?.Any() != true)
            {
                return;
            }

            foreach (var device in devices)
            {
                switch (device.CommunicateType)
                {
                    case CommunicateType.ModbusRtu:
                        await AddRtuAsync(device.ConnectionOptions);
                        break;
                    case CommunicateType.ModbusTcp:
                        await AddTcpAsync(device.ConnectionOptions);
                        break;
                    default:
                        break;
                }
            }
        }

        public static async Task<int> ReconnectModbusMastersAsync()
        {
            await DisconnectAllModbusMastersAsync();

            var onlineCount = 0;
            foreach (var client in ModbusMasterPool.Values)
            {
                onlineCount += await client.ConnectAsync() ? 1 : 0;
            }

            UiGlobalContext.Instance.MainSlaveId = ModbusMasterPool.Values.Min(m => m.Options.SlaveId);
            return onlineCount;
        }

        public static async Task ClearModbusMastersAsync()
        {
            await DisconnectAllModbusMastersAsync();
            foreach (var client in ModbusMasterPool.Values)
            {
                await client?.CloseAsync();
            }

            ModbusMasterPool.Clear();
            StoragePool.Clear();
            ConsistencyContext.Instance.Clear();
        }

        public static async Task DisconnectAllModbusMastersAsync()
        {
            try
            {
                var excepts = new List<string>();
                foreach (var clientPair in ModbusMasterPool)
                {
                    if (clientPair.Value is not IModbusObject)
                    {
                        excepts.Add(clientPair.Key);
                        continue;
                    }

                    await clientPair.Value.DisconnectAsync();
                }

                foreach (var storagePair in StoragePool)
                {
                    if (excepts.Contains(storagePair.Key))
                    {
                        continue;
                    }

                    storagePair.Value.ClearBuffers();
                }

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "CommunicateAdapterPool.DisconnectAll");
            }

        }

        public static async Task<IConnectClient> AddRtuAsync(EmsConnectionOption rtuOptions)
        {
            var key = rtuOptions.GetKey();
            if (ModbusMasterPool.TryGetValue(key, out IConnectClient rtu))
            {
                await rtu.DisconnectAsync();
            }

            StoragePool[key] = new ModbusDataStorage(key);
            var client = new ModbusRtuObject();
            await client.InitializeAsync(rtuOptions);
            ModbusMasterPool[key] = client;

            return client;
        }

        public static async Task<IConnectClient> AddTcpAsync(EmsConnectionOption tcpOptions)
        {
            var key = tcpOptions.GetKey();
            if (ModbusMasterPool.TryGetValue(key, out IConnectClient tcp))
            {
                await tcp.DisconnectAsync();
            }

            StoragePool[key] = new ModbusDataStorage(key);
            var client = new ModbusTcpObject();
            await client.InitializeAsync(tcpOptions);
            ModbusMasterPool[key] = client;

            return client;
        }

        public static async Task<IConnectClient> AddMqttClientAsync(string sn, EmsConnectionOption options)
        {
            var key = options.GetKey();
            if (ModbusMasterPool.TryGetValue(key, out IConnectClient mqtt))
            {
                await mqtt.DisconnectAsync();
            }

            StoragePool[key] = new ModbusDataStorage(key);
            var client = new MqttTcpClientObject(sn);
            await client.InitializeAsync(options);
            ModbusMasterPool[key] = client;

            return client;
        }

        public static async Task<IConnectClient> AddMqttClientAsync(MqttTcpClientObject client)
        {
            var options = client.OriginConfig;
            var key = options.GetKey();
            if (ModbusMasterPool.TryGetValue(key, out IConnectClient mqtt))
            {
                await mqtt.DisconnectAsync();
            }

            StoragePool[key] = new ModbusDataStorage(key);

            await client.InitializeAsync(options);
            ModbusMasterPool[key] = client;

            return client;
        }

        #endregion

        #region mqtt
        public static async Task<MqttClientObject> InitializeMqttClientAsync(string key, string sn, MqttEnvironmentConfig config, List<string> topics)
        {
            if (MqttClientPool.TryGetValue(sn, out MqttClientObject client))
            {
                await client.CloseAsync();
                MqttClientPool.Remove(sn);
            }

            var mqttEnvs = ConfigContext.GenericConfig?.MqttConfig
                ?.EnvironmentConfigs;
            if (mqttEnvs?.Any() != true)
            {
                return default;
            }

            var newClient = new MqttClientObject(sn, key);
            await newClient.InitializeAsync(key, config, topics);
            MqttClientPool[sn] = newClient;

            return newClient;
        }

        public static async Task<int> ConnectMqttClientsAsync()
        {
            var connectCount = 0;
            foreach (var client in MqttClientPool.Values)
            {
                connectCount += (await client.ConnectAsync() ? 1 : 0);
            }

            return connectCount;
        }

        public static async Task DisconnectAllMqttClientsAsync()
        {
            try
            {

                foreach (var client in MqttClientPool.Values)
                {
                    await client.DisconnectAsync();
                }

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "CommunicateAdapterPool.DisconnectAllMqttClientsAsync");
            }

        }

        public static async Task ClearMqttClientsAsync()
        {
            await DisconnectAllMqttClientsAsync();
            foreach (var client in MqttClientPool.Values)
            {
                await client.CloseAsync();
            }

            MqttClientPool.Clear();
        }
        #endregion
    }
}
