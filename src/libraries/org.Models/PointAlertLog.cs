using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public class PointAlertLog
    {
        public string ModbusKey { get; set; }
        public int FunctionCode { get; set; }
        public ushort Address { get; set; }

        /// <summary>
        /// 告警历史，仅告警的历史
        /// </summary>
        public ConcurrentStack<KeyValuePair<DateTime, ushort>> ValueLogs { get; set; } = new();
    }
}
