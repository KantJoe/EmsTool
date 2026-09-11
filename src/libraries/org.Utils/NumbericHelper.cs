using org.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace org.Utils
{
    public static class NumbericHelper
    {
        public static bool HitAlertThreshold(
            ushort currentValue, ushort? lastValue, AlertCondition condition, ushort threshold, ushort thresholdLeft, ushort thresholdRight)
        {
            switch (condition)
            {
                case AlertCondition.LessThan:
                    return currentValue < threshold;
                case AlertCondition.LessThanOrEqual:
                    return currentValue <= threshold;
                case AlertCondition.Equal:
                    return currentValue == threshold;
                case AlertCondition.NotEqual:
                    return currentValue != threshold;
                case AlertCondition.GreaterThan:
                    return currentValue > threshold;
                case AlertCondition.GreaterThanOrEqual:
                    return currentValue >= threshold;
                case AlertCondition.Between:
                    return currentValue > thresholdLeft && currentValue <= thresholdRight;
                case AlertCondition.NotBetween:
                    return currentValue <= thresholdLeft || currentValue > thresholdRight;
                case AlertCondition.Changed:
                    return !lastValue.HasValue || lastValue.Value != currentValue;
                default:
                    return false;
            }
        }

        public static bool HitAlertTime(TimeSpan ts, AlertTimes time)
        {
            switch (time)
            {
                case AlertTimes.None:
                    return false;
                case AlertTimes.Once:
                    return true;
                case AlertTimes.OncePerMinute:
                    return ts.TotalMinutes >= 1;
                case AlertTimes.OncePerHour:
                    return ts.TotalHours >= 1;
                case AlertTimes.OncePerDay:
                    return ts.TotalDays >= 1;
                case AlertTimes.Always:
                default:
                    return true;
            }
        }
    }
}
