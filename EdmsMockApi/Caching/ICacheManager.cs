using System;
using System.Threading.Tasks;

namespace EdmsMockApi.Caching
{
    public interface ICacheManager : IDisposable
    {
        void Clear();

        T Get<T>(string key, Func<T> acquire, int? cacheTime = null);

        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Set(string key, object data, int cacheTime);
    }
}