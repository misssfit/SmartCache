using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCache
{
    public static class CacheItemExtensions
    {
        public static string Key(this ICacheItem item)
        {
            if (item is ICategorisedCacheItem)
            {
                return Key(item as ICategorisedCacheItem);
            }
            return Key(item.GetType(), item.Id);
        }

        public static string Key<T>(this IEnumerable<T> item) where T : ICacheItem
        {
            return Key(typeof(T));
        }

        public static string Key(this ICategorisedCacheItem item)
        {
            return Key(item.GetType(), item.Id, item.Category);
        }

        //public static string Key<T>(this IEnumerable<T> item) where T : ICategorisedCacheItem
        //{
        //    return Key(typeof(T));
        //}

        public static string Key(Type type, int id, string category = "")
        {
            return string.Format("{0}@{1}@{2}", type.Name, category, id);
        }

        public static string Key(Type type)
        {
            return string.Format("{0}@@ALL", type.Name);
        }
    }
}
