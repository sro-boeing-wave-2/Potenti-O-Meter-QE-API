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
using QuizServer.Service;

namespace QuizServer
{
    public class Program
    {
       // private readonly static IGraphService graphService;

        public static void Main(string[] args)
        {
            //private IGraphService graphService;
            //try
            //{
            //    DotNetEnv.Env.Load("./machine_config/machine.env");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //Console.WriteLine(System.Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4"));
            CreateWebHostBuilder(args).Build().Run();
            ////private IGraphServicec;
            //IGraphService graphService = new GraphService();
            //MessageBusService conceptMapListener = new MessageBusService(graphService);
            //conceptMapListener.CreateQuestionConceptMap();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

