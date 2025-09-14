using System;
using System.Threading;

namespace ConsoleApp1
{
    delegate void DoSomething();

    public class Program
    {
        static void Main()
        {
            DoSomething del = () =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
            };

            var iar = del.BeginInvoke(AsyncCallbackMethod, "hakuna");
            del.EndInvoke(iar);
        }

        static void AsyncCallbackMethod(IAsyncResult ar)
        {
            Console.WriteLine(ar.AsyncState.ToString());
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
        }

    }
}