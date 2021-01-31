using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Api.Gw.Ocelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var ocelotConf = env == "Development" ?
                Path.Combine("configuration", "configuration.local.json") :
                Path.Combine("configuration", "configuration.k8s.json");

            IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args);
            builder.ConfigureServices(s => s.AddSingleton(builder))
                .ConfigureAppConfiguration(ic => ic.AddJsonFile(ocelotConf))
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, loggingbuilder) =>
                {
                    loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingbuilder.AddConsole();
                    loggingbuilder.AddDebug();
                })
                .UseSerilog((builderContext, config) =>
                {
                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                });
            IWebHost host = builder.Build();
            return host;
        }
    }
}
