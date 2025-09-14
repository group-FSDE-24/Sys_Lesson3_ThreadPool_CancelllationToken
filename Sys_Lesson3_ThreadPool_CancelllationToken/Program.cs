using Sys_Lesson3_ThreadPool_CancelllationToken.Models;
using System.Diagnostics;

namespace Sys_Lesson3_ThreadPool_CancelllationToken;



class Program
{
    delegate void DoSomething();

    static void Main(string[] args)
    {
        //////////////////////////////////////////////////////////////////

        // WaitCallBackMethod 

        // ThreadPool.QueueUserWorkItem(WaitCallBackMethod);
        // ThreadPool.QueueUserWorkItem(WaitCallBackMethod, "Burhan");
        // Console.ReadKey();

        //////////////////////////////////////////////////////////////////

        // WaitCallBackMethod  With Class
        // Student student = new Student()
        // {
        //     FirstName = "Kamran",
        //     LastName = "Karimzada"
        // };

        // ThreadPool.QueueUserWorkItem(WaitCallBackMethodForStudent, student);
        // ThreadPool.QueueUserWorkItem(PrintStudent, student, preferLocal: false);

        // Console.ReadKey();

        //////////////////////////////////////////////////////////////////
        // GetMinThreads | GetMaxThreads | GetAvailableThreads

        // ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThread);
        // 
        // Console.WriteLine($"WorkerThreads: {workerThreads}");
        // Console.WriteLine($"CompletionPortThread: {completionPortThread}");
        // 
        // ThreadPool.GetMaxThreads(out int workerThreads2, out int completionPortThread2);
        // 
        // Console.WriteLine($"WorkerThreads2: {workerThreads2}");
        // Console.WriteLine($"CompletionPortThread2: {completionPortThread2}");
        // 
        // ThreadPool.GetAvailableThreads(out int workerThreads3, out int completionPortThread3);
        // 
        // Console.WriteLine($"WorkerThreads3: {workerThreads3}");
        // Console.WriteLine($"CompletionPortThread3: {completionPortThread3}");
        // 
        // Console.WriteLine($"ThreadCount: {ThreadPool.ThreadCount}");

        //////////////////////////////////////////////////////////////////

        // Thread vs ThreadPool

        // {
        //     int operations = 500;
        //     var watch = new Stopwatch();
        // 
        //     watch.Start();
        //     UseThread(operations);
        //     watch.Stop();
        // 
        //     Console.WriteLine("Thread Milliseconds :" + watch.ElapsedMilliseconds);
        // 
        //     watch.Reset();
        // 
        // 
        //     watch.Start();
        //     UseThreadPool(operations);
        //     watch.Stop();
        // 
        // 
        //     Console.WriteLine("ThreadPool Milliseconds :" + watch.ElapsedMilliseconds);
        // 
        // 
        //   
        // }

        //////////////////////////////////////////////////////////////////

        // // Not Support

        // DoSomething del = () => {
        //     Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        //     Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
        // };
        // 
        // del.BeginInvoke(AsyncCallbackMethod, "hakuna");

        //////////////////////////////////////////////////////////////////

        // // Example For Hw
        // Console.OutputEncoding = System.Text.Encoding.Unicode;
        //
        // Console.Write("Mətni daxil edin: ");
        // string text = Console.ReadLine();
        //
        //
        // // Şifrələmə
        // char[] encrypted = XorProcess(text);
        // Console.WriteLine("Şifrələnmiş (XOR): " + new string(encrypted));
        //
        // // Deşifrələmə (eyni funksiya ilə)
        // char[] decrypted = XorProcess(new string(encrypted));
        // Console.WriteLine("Deşifrələnmiş: " + new string(decrypted));

        //////////////////////////////////////////////////////////////////

        //  Cancellation Token

        using var cts = new CancellationTokenSource();

        ThreadPool.QueueUserWorkItem(_ =>
        {
            MoneyTransfer(cts.Token);

        });

        Console.ReadKey();

        cts.Cancel();

        while (true)
        {
           
        }

    }

    public static void WaitCallBackMethod(object? state)
    {
        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"State: {state}");
        Console.WriteLine($"IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
        Console.WriteLine($"IsBackground: {Thread.CurrentThread.IsBackground}");
    }

    public static void WaitCallBackMethodForStudent(object? state)
    {
        var student = state as Student;

        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"Name: {student.FirstName}");
        Console.WriteLine($"Surname: {student.LastName}");
    }

    public static void PrintStudent(Student student)
    {
        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"Name: {student.FirstName}");
        Console.WriteLine($"Surname: {student.LastName}");
    }

    public static void UseThread(int operation)
    {
        using var countdown = new CountdownEvent(operation);
        Console.WriteLine("Threads !!!");

        for (int i = 0; i < operation; i++)
        {
            var thread = new Thread(() =>
            {
                Console.Write($"{Thread.CurrentThread.ManagedThreadId} ");

                Thread.Sleep(100);
                countdown.Signal();
            });

            thread.Start();
        }

        countdown.Wait();
        Console.WriteLine();
    }

    public static void UseThreadPool(int operation)
    {
        using var countdown = new CountdownEvent(operation);

        Console.WriteLine("ThreadPools !!!");

        for (int i = 0; i < operation; i++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.Write($"{Thread.CurrentThread.ManagedThreadId} ");

                Thread.Sleep(100);
                countdown.Signal();
            });
        }

        countdown.Wait();
        Console.WriteLine();
    }

    public static void AsyncCallbackMethod(IAsyncResult ar)
    {
        Console.WriteLine(ar.AsyncState.ToString());
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
    }


    #region EncryptAndDecrypt

    const char key = 'K';
    static char[] XorProcess(string input)
    {
        char[] chars = input.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = (char)(chars[i] ^ key);
        }

        return chars;
    }

    #endregion

    public static void MoneyTransfer(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Id: {Thread.CurrentThread.ManagedThreadId}");

        try
        {
            for (var i = 0; i < 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine("Send Money");

                Thread.Sleep(2000);

                Console.WriteLine("Take Money");
            }
        }
        catch (OperationCanceledException oex)
        {
            Console.WriteLine(oex.Message);
        }
    }

}