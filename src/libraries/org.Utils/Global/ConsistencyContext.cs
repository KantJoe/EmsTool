using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace org.Utils.Global
{
    public class ConsistencyContext
    {
        public static ConsistencyContext Instance { get; private set; }

        private ConsistencyContext()
        {
            _lockDictionary = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        static ConsistencyContext()
        {
            Instance = new ConsistencyContext();
        }

        private ConcurrentDictionary<string, SemaphoreSlim> _lockDictionary;

        public SemaphoreSlim GetInstance(int initCount = 1, int maxCount = 1, string lockKey = null)
        {
            if (string.IsNullOrEmpty(lockKey))
            {
                return new SemaphoreSlim(initCount, maxCount);
            }

            if (_lockDictionary.TryGetValue(lockKey, out var instance))
            {
                return instance;
            }

            instance = new SemaphoreSlim(initCount, maxCount);
            _lockDictionary[lockKey] = instance;
            return instance;
        }

        public bool ReleaseSemaphore(string key, Action releasingAction = null, Action releasedAction = null, int releaseCount = 1)
        {
            if (!_lockDictionary.TryGetValue(key, out var @lock))
            {
                return false;
            }

            releasingAction?.Invoke();
            if (@lock.CurrentCount == 0)
            {
                if (@lock.Wait(-1) && @lock.Release() == 0)
                {
                    releasedAction?.Invoke();
                    return true;
                }

                return false;
            }

            return true;
        }

        public void Clear()
        {
            foreach (var item in _lockDictionary.Values)
            {
                try
                {
                    item?.Dispose();
                }
                catch (Exception ex)
                {
                    LogFactory.Error(ex, "ConsistencyContext.Clear");
                }
            }

            _lockDictionary.Clear();
        }
    }
}
