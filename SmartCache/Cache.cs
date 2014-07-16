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
        private List<string> _queue = new List<string>();

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
            MemoryCache.Default.Add(item.Key(), item, AbsoluteExpiration(item));
        }


        public void Add<T>(IEnumerable<T> item) where T : ICacheItem
        {
            MemoryCache.Default.Add(item.Key(), item, AbsoluteExpiration(item));
        }

        public void Update<T>(T item) where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof(T), item.Id);
            MemoryCache.Default.Remove(key);
            MemoryCache.Default.Add(item.Key(), item, AbsoluteExpiration(item));
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

        private DateTime AbsoluteExpiration<T>(T item)
        {
            var date = DateTime.Now;
            if (_registeredTypes.ContainsKey(typeof(T)) == true)
            {
                date = date.AddMinutes(_registeredTypes[typeof(T)].Minutes());
            }
            else
            {
                date = date.AddMinutes(CacheLevel.L1.Minutes());
            }

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

        private object _o = new object();
        private object _o2 = new object();

        public T Get<T>(int id, Func<T> getAction) where T : ICacheItem
        {
            var key = CacheItemExtensions.Key(typeof(T), id);

            if (MemoryCache.Default.Contains(key) == false)
            {
                //if (_queue.Contains(key) == false)
                if(Start(key)==true)
                {
                    //lock (_queue)
                    {
                      //  if (_queue.Contains(key) == false)
                        {
                   //         _queue.Add(key);
                            var item = getAction();
                            Add(item);
                            lock (_queue)
                            {
                                _queue.Remove(key);
                            }
                            Console.WriteLine("Ev" + key);
                            return (T) item;
                        }
                    }
                }

                if (MemoryCache.Default.Contains(key) == false && _queue.Contains(key) == true)
                {
                    var contains = true;
                    while (contains == true)
                    {
                        Thread.Sleep(10);
                        lock (_queue)
                        {
                            contains = _queue.Contains(key);
                        }
                    }
                }
            }
            return (T)MemoryCache.Default.Get(key);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool Start(string key)
        {
            if (_queue.Contains(key))
            {
                return false;
            }
            else
            {
                _queue.Add(key);
                return true;

            }
        }


    }
}
