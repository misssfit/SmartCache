using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCache
{
    public interface ISmartCache
    {
        void RegisterTypes(CacheLevel level, params Type[] types);
        void Add<T>(T item) where T : ICacheItem;
        void Add<T>(IEnumerable<T> item) where T : ICacheItem;

        void Update<T>(T item) where T : ICacheItem;

        T Get<T>(int id) where T : ICacheItem;
        T Get<T>(int id, Func<T> getAction) where T : ICacheItem;

        T Pop<T>(int id) where T : ICacheItem;

        T Get<T>(int id, string category) where T : ICategorisedCacheItem;
        T Get<T>(int id, string category, Func<T> getAction) where T : ICategorisedCacheItem;
        T Pop<T>(int id, string category) where T : ICategorisedCacheItem;

        IEnumerable<T> GetAll<T>() where T : ICacheItem;

        void Evict<T>(int id) where T : ICacheItem;
        void Evict<T>(int id, string category) where T : ICategorisedCacheItem;

    }
}
