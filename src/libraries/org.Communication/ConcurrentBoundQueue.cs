using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.Communication
{
    /// <summary>
    /// ConcurrentBoundQueue主要目的
    /// 1、收集定时轮询数据
    /// 2、有效时间内始终提供最新数据
    /// 3、定时清理最旧的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ConcurrentBoundQueue<T> : ConcurrentQueue<T>
    {
        private int _defaultMaxCapcity = 5;

        public int MaxCapacity { get; set; }

        public ConcurrentBoundQueue() : base()
        {
            MaxCapacity = _defaultMaxCapcity;
        }

        public ConcurrentBoundQueue(int maxCapacity = 50) : base()
        {
            if (maxCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Capacity must be greater than zero.");
            }

            MaxCapacity = maxCapacity;
        }

        public ConcurrentBoundQueue(IEnumerable<T> values, int maxCapacity = 50) : base(values)
        {
            MaxCapacity = maxCapacity;
        }

        public void TryEnqueue(T item, bool autoUpdate = true)
        {
            Enqueue(item);
            if (autoUpdate)
            {
                ClearTail();
            }
        }

        public bool GetLast(out T result)
        {
            result = default;
            return !IsEmpty && TryDequeue(out result);
        }

        public void ClearTail()
        {
            for (int index = Count; index > MaxCapacity; index--)
            {
                TryDequeue(out _);
            }
        }

    }
}
