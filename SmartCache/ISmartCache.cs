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
        T Pop<T>(int id) where T : ICacheItem;

        IEnumerable<T> GetAll<T>() where T : ICacheItem;

    }
}
