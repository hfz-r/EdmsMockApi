using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace EdmsMockApi.Caching
{
    public class MemoryCacheManager : ILocker, ICacheManager
    {
        private readonly IMemoryCache _cache;

        protected static readonly ConcurrentDictionary<string, bool> AllKeys;
        protected CancellationTokenSource CancellationTokenSource;

        static MemoryCacheManager()
        {
            AllKeys = new ConcurrentDictionary<string, bool>();
        }

        public MemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache;
            CancellationTokenSource = new CancellationTokenSource();
        }

        #region Utilities

        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(TimeSpan cacheTime)
        {
            var options = new MemoryCacheEntryOptions()
                .AddExpirationToken(new CancellationChangeToken(CancellationTokenSource.Token))
                .RegisterPostEvictionCallback(PostEviction);

            options.AbsoluteExpirationRelativeToNow = cacheTime;

            return options;
        }

        protected string AddKey(string key)
        {
            AllKeys.TryAdd(key, true);

            return key;
        }

        protected string RemoveKey(string key)
        {
            TryRemoveKey(key);

            return key;
        }

        protected void TryRemoveKey(string key)
        {
            if (!AllKeys.TryRemove(key, out _))
                AllKeys.TryUpdate(key, false, true);
        }

        private void ClearKeys()
        {
            foreach (var key in AllKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
                RemoveKey(key);
        }

        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            if (reason == EvictionReason.Replaced)
                return;

            ClearKeys();

            TryRemoveKey(key.ToString());
        }

        #endregion

        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;

            var result = acquire();

            if ((cacheTime ?? 60) > 0)
                Set(key, result, cacheTime ?? 60);

            return result;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;

            var result = await acquire();

            if ((cacheTime ?? 60) > 0)
                Set(key, result, cacheTime ?? 60);

            return result;
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data != null)
                _cache.Set(AddKey(key), data, GetMemoryCacheEntryOptions(TimeSpan.FromMinutes(cacheTime)));
        }

        public virtual bool IsSet(string key)
        {
            return _cache.TryGetValue(key, out object _);
        }

        public bool PerformActionWithLock(string key, TimeSpan expirationTime, Action action)
        {
            if (!AllKeys.TryAdd(key, true))
                return false;

            try
            {
                _cache.Set(key, key, GetMemoryCacheEntryOptions(expirationTime));

                action();

                return true;
            }
            finally
            {
                Remove(key);
            }
        }

        public virtual void Remove(string key)
        {
            _cache.Remove(RemoveKey(key));
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matchesKeys = AllKeys.Where(p => p.Value).Select(p => p.Key).Where(key => regex.IsMatch(key)).ToList();

            foreach (var key in matchesKeys)
                _cache.Remove(RemoveKey(key));
        }

        public virtual void Clear()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = new CancellationTokenSource();
        }

        public virtual void Dispose()
        {
            //nothing special
        }
    }
}