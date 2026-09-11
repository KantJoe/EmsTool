using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    public enum AlertCondition
    {
        None = 0,
        LessThan,
        LessThanOrEqual,
        Equal,
        NotEqual,
        GreaterThanOrEqual,
        GreaterThan,
        /// <summary>
        /// (left,right]
        /// </summary>
        Between,
        /// <summary>
        /// (left,right]
        /// </summary>
        NotBetween,
        Changed
    }

    public enum AlertTimes
    {
        None,
        /// <summary>
        /// 仅一次
        /// </summary>
        Once,
        /// <summary>
        /// 每分钟一次
        /// </summary>
        OncePerMinute,
        /// <summary>
        /// 每小时一次
        /// </summary>
        OncePerHour,
        /// <summary>
        /// 每天一次
        /// </summary>
        OncePerDay,
        /// <summary>
        /// 总是告警
        /// </summary>
        Always
    }
}
