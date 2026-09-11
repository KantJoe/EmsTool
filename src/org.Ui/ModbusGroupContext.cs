using CommunityToolkit.Mvvm.Messaging;
using org.Models;
using org.Models.Messagings;
using org.Ui.Messagings;
using System;
using System.Collections.Generic;

namespace org.Ui
{
    public class ModbusGroupContext
    {
        public static ModbusGroupContext Instance { get; private set; }
        /// <summary>
        /// 分组消息
        /// key：functioncode_startAddress
        /// </summary>
        public Dictionary<string, Action<ReadOnlyMemory<ushort>, string>> Groups { get; private set; }

        private ModbusGroupContext()
        {
            Groups = new Dictionary<string, Action<ReadOnlyMemory<ushort>, string>>();
        }

        static ModbusGroupContext()
        {
            Instance = new ModbusGroupContext();
        }

        public static void SendMessage(ModbusFunctionCode functionCode, int startAddr, ReadOnlyMemory<ushort> buffer, string token = null)
        {
            if (!Instance.Groups
                .TryGetValue($"{(int)functionCode}_{startAddr}",
                out Action<ReadOnlyMemory<ushort>, string> msgAction))
            {
                return;
            }

            msgAction(buffer, token);
        }
    }
}
