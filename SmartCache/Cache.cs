using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCache
{
    public class Cache : Singleton<Cache>, ISmartCache
    {
        private readonly Dictionary<Type, CacheLevel> _registeredTypes = new Dictionary<Type, CacheLevel>();
        //private Dictionary<string, object> _queue = new Dictionary<string, object>();

        public void RegisterTypes(CacheLevel level, params Type[] types)
        {
            foreach (var type in types)
            {
                if (_registeredTypes.ContainsKey(type) == false)
                {
                    _registeredTypes.Add(type, level);
                }
                else
                {
                    _registeredTypes[type] = level;
                }
            }
        }


        public void Add<T>(T item) where T : ICacheItem
        {
            MemoryCache.Default.Add(item.Key(), item, AbsoluteExpiration<T>());
        }


        public void Add<T>(IEnumerable<T> item) where T : ICacheItem
        {
            MemoryCache.Default.Add(item.Key(), item, AbsoluteExpiration<T>());
        }

        public void Update<T>(T item) where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof(T), item.Id);
            MemoryCache.Default.Remove(key);
            MemoryCache.Default.Add(item.Key(), item, AbsoluteExpiration<T>());
        }

        public T Get<T>(int id) where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof(T), id);
            var result = (T)MemoryCache.Default.Get(key);
            return result;
        }

        public IEnumerable<T> GetAll<T>() where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof(T));
            var result = (IEnumerable<T>)MemoryCache.Default.Get(key);
            return result;
        }

        private DateTime AbsoluteExpiration<T>()
        {
            var date = DateTime.Now;
            date = date.AddMinutes(_registeredTypes.ContainsKey(typeof(T)) == true 
                ? _registeredTypes[typeof(T)].Minutes() 
                : CacheLevel.L1.Minutes());

            return date;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public T Pop<T>(int id) where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof(T), id);
            var result = (T)MemoryCache.Default.Get(key);

            MemoryCache.Default.Remove(key);

            return result;
        }


        public void Evict<T>(int id) where T : ICacheItem
        {
            throw new NotImplementedException();
        }


        public T Get<T>(int id, string category) where T : ICategorisedCacheItem
        {
            throw new NotImplementedException();
        }

        public T Get<T>(int id, string category, Func<T> getAction) where T : ICategorisedCacheItem
        {
            throw new NotImplementedException();
        }

        public T Pop<T>(int id, string category) where T : ICategorisedCacheItem
        {
            throw new NotImplementedException();
        }

        public void Evict<T>(int id, string category) where T : ICategorisedCacheItem
        {
            throw new NotImplementedException();
        }

        public T Get<T>(int id, Func<T> getAction) where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof (T), id);
            if (MemoryCache.Default.Contains(key) == false)
            {
                MemoryCache.Default.Add(key, new Lazy<T>(getAction, true), AbsoluteExpiration<T>());

            }
            return (MemoryCache.Default.Get(key) as Lazy<T>).Value;
        }
    }
}
