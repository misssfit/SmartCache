using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCache;

namespace TestApp
{
    class A : ICacheItem
    {

        public int Id { get; set; }
    }


    class B : ICategorisedCacheItem
    {
        public int Id { get; set; }
        public string Category { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var c = new Cache();
            c.Add(new A { Id = 1 });
            c.Add(new B { Id = 1, Category = "AA" });
            c.Add(new B { Id = 1, Category = "BB" });

            var a = c.Pop<A>(1);
            var b = c.Pop<B>(1);

            c.RegisterTypes(CacheLevel.L2, typeof(A));
            c.RegisterTypes(CacheLevel.L3, typeof(B));

            c.Add(new A { Id = 1 });
            c.Add(new B { Id = 1, Category = "AA" });
            c.Add(new B { Id = 1, Category = "BB" });

            var aa = c.Get<A>(1);
            var bb = c.Get<B>(1);
        }
    }
}
