using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using QuizServer.Service;
using QuizServer.Data;

namespace QuizServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(o => o.AddPolicy("CorsPolicy", b => 
                b.AllowCredentials()
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            ));


            services.Configure<Settings>(Options =>
            {
                Options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                Options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IQuizEngineService, QuizEngineService>();
            services.AddSingleton<IResultContext, ResultContext>();
            services.AddSingleton<IResultService, ResultService>();
            services.AddSingleton<IGraphService, GraphService>();
            services.AddSingleton<IMessageBusService, MessageBusService>();
            services.AddSignalR();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMessageBusService messageBusService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<QuestionHub>("/start");
            });
            
            //var hubConfiguration = new HubConfiguration();
            //hubConfiguration.EnableDetailedErrors = true;
            //app.MapSignalR(hubConfiguration);
            app.UseMvc();
            
        }
    }
}
