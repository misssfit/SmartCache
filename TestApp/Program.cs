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
            c.Add(new A {Id = 1});
            c.Add(new B {Id = 1, Category = "AA"});
            c.Add(new B {Id = 1, Category = "BB"});

            var a = c.Pop<A>(1);
            var b = c.Pop<B>(1);

            c.RegisterTypes(CacheLevel.L2, typeof (A));
            c.RegisterTypes(CacheLevel.L3, typeof (B));

            c.Add(new A {Id = 1});
            c.Add(new B {Id = 1, Category = "AA"});
            c.Add(new B {Id = 1, Category = "BB"});

            var aa = c.Get<A>(1);
            var bb = c.Get<B>(1);
                Stopwatch sw = new Stopwatch();
                sw.Start();
              var t1=  Task.Factory.StartNew(() =>
                {
                    var x = c.Get(6, () => TestMethod(6));
                   // Thread.Sleep(200);
                    Console.WriteLine(null== x);

                    Console.WriteLine("1 "+x);
                });
              var t2 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get(6, () => TestMethod(6));
                   // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("2 " + x);

                });
              var t3 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get(6, () => TestMethod(6));
                   // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("3 " + x);

                });
              var t4 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get(88, () => TestMethod(88));
                   // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("4 " + x);

                });
              var t6 = Task.Factory.StartNew(() =>
              {
                  var x = c.Get(88, () => TestMethod(88));
                 // Thread.Sleep(200);
                  Console.WriteLine(null == x);

                  Console.WriteLine("6 " + x);

              });
              var t5 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get(6, () => TestMethod(6));
                   // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("5 " + x);

                });
                t1.Wait();
                t2.Wait();
                t3.Wait();
                t4.Wait();
                t6.Wait();
                t5.Wait();
                sw.Stop();
                Console.WriteLine(sw.Elapsed);





                Stopwatch sw1 = new Stopwatch();
                sw1.Start();
                var t11 = Task.Factory.StartNew(() =>
                {
                    var x = TestMethod(60);
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("1 " + x);
                });
                var t21 = Task.Factory.StartNew(() =>
                {
                    var x = TestMethod(60);
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("2 " + x);

                });
                var t31 = Task.Factory.StartNew(() =>
                {
                    var x = TestMethod(60);
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("3 " + x);

                });
                var t41 = Task.Factory.StartNew(() =>
                {
                    var x = TestMethod(99);
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("4 " + x);

                });
                var t61 = Task.Factory.StartNew(() =>
                {
                    var x = TestMethod(99);
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("6 " + x);

                });
                var t51 = Task.Factory.StartNew(() =>
                {
                    var x = TestMethod(60);
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("5 " + x);

                });
                t11.Wait();
                t21.Wait();
                t31.Wait();
                t41.Wait();
                t61.Wait();
                t51.Wait();
                sw1.Stop();
                Console.WriteLine(sw1.Elapsed);




                Stopwatch sw2 = new Stopwatch();
                sw2.Start();
                var t12 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get<A>(600);
                    if (x == null)
                    {
                        x = TestMethod(600);
                        c.Add(x);
                    }
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("1 " + x);
                });
                var t22 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get<A>(600);
                    if (x == null)
                    {
                        x = TestMethod(600);
                        c.Add(x);
                    }
                    // Thread.Sleep(200);
                    Console.WriteLine(null == x);

                    Console.WriteLine("2 " + x);

                });
                var t32 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get<A>(600);
                    if (x == null)
                    {
                        x = TestMethod(600);
                        c.Add(x);
                    }
                    Console.WriteLine(null == x);

                    Console.WriteLine("3 " + x);

                });
                var t42 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get<A>(6001);
                    if (x == null)
                    {
                        x = TestMethod(6001);
                        c.Add(x);
                    }
                    Console.WriteLine(null == x);

                    Console.WriteLine("4 " + x);

                });
                var t62 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get<A>(6001);
                    if (x == null)
                    {
                        x = TestMethod(6001);
                        c.Add(x);
                    }
                    Console.WriteLine(null == x);

                    Console.WriteLine("6 " + x);

                });
                var t52 = Task.Factory.StartNew(() =>
                {
                    var x = c.Get<A>(600);
                    if (x == null)
                    {
                        x = TestMethod(600);
                        c.Add(x);
                    }
                    Console.WriteLine(null == x);

                    Console.WriteLine("5 " + x);

                });
                t12.Wait();
                t22.Wait();
                t32.Wait();
                t42.Wait();
                t62.Wait();
                t52.Wait();
                sw2.Stop();
                Console.WriteLine(sw2.Elapsed);










            }
        catch(Exception e)
    {
        
    }

    }

        private static A TestMethod(int p)
        {
            Thread.Sleep(1000);
            return new A() {Id = p};
        }
    }
}
