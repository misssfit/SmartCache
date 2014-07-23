using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartCache;

namespace TestApp
{
    class A : ICacheItem
    {

        public int Id { get; set; }
        public override string ToString()
        {
            return "A#" + Id;
        }
    }


    class B : ICategorisedCacheItem
    {
        public int Id { get; set; }
        public string Category { get; set; }
    }

    class Program
    {
        private static void Main(string[] args)
        {
            try
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

                var res1 = GetValue(1000, 1050, p => c.Get(p, () => TestMethod(p)));
                // var res2= GetValue(2000, 2200, TestMethod);
                var res3 = GetValue(3000, 3050, p =>
                  {
                      var x = c.Get<A>(p);
                      if (x == null)
                      {
                          x = TestMethod(p);
                          c.Add(x);
                      }
                      return x;
                  }

                  );

                Console.WriteLine(res1);
                // Console.WriteLine(res2);
                Console.WriteLine(res3);











            }
            catch (Exception e)
            {

            }

        }

        private static TimeSpan GetValue(int start, int stop, Func<int, A> aFunc)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var tasks = new List<Task>();
            for (int q = 0; q < 2; q++)
            {

                for (int i = start; i < stop; i++)
                {
                    for (int k = 0; k < 15; k++)
                    {
                        int i1 = i;
                        var t1 = Task.Factory.StartNew(() =>
                        {
                            var x = aFunc(i1);
                            //Console.WriteLine(null == x);
                            //  Console.WriteLine(i1 + " ** " + x);

                            // Thread.Sleep(150);
                        });
                        tasks.Add(t1);
                    }
                }
            }

           
                Task.WaitAll(tasks.ToArray());
            Console.WriteLine(tasks.Count());

            sw.Stop();
            // Console.WriteLine(sw.Elapsed);
            return sw.Elapsed;
        }
        //private static int count = 0;
        private static A TestMethod(int p)
        {
            //count++;
            // Console.WriteLine("ACCESS DAL $$" + p);
            // Console.WriteLine("~~" + count);

            Thread.Sleep(550);

            Console.WriteLine("Get" + p);


            return new A() { Id = p };
        }
    }
}
