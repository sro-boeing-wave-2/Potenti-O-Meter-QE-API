using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace QuizServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var numbers = new List<int>(Enumerable.Range(1, 10));
            //numbers.Shuffle();
            //Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", numbers.GetRange(0, 10)));
            //numbers.Shuffle();
            //Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", numbers.GetRange(0, 10)));
            //numbers.Shuffle();
            //Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", numbers.GetRange(0, 10)));
            //numbers.Shuffle();
            //Console.WriteLine("The winning numbers are: {0}", string.Join(",  ", numbers.GetRange(0, 10)));
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    //public static class ThreadSafeRandom
    //{
    //    [ThreadStatic] private static Random Local;

    //    public static Random ThisThreadsRandom
    //    {
    //        get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    //    }
    //}

    //static class MyExtensions
    //{
    //    public static void Shuffle<T>(this List<T> list)
    //    {
    //        int n = list.Count;
    //        while (n > 1)
    //        {
    //            n--;
    //            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
    //            T value = list[k];
    //            list[k] = list[n];
    //            list[n] = value;
    //        }
    //    }
    //}
}

