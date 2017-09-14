using System;
using System.IO;
using BaseApp.Common.Logs;
using BaseApp.Web.Code.Infrastructure.Logs;
using BaseApp.Web.Code.Infrastructure.Logs.LogRenderers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.LayoutRenderers;

namespace BaseApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                var host = BuildWebHost(args);
                host.Run();
            }
            catch (Exception ex)
            {
                new StartupLogger(currentDirectory).ErrorException(ex, "Main method");
                throw;
            }
        }

        // neccessary for EF Tooling
        public static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(ConfigConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
        }

        private static void ConfigureLogging(WebHostBuilderContext hostingContext, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();

            LayoutRenderer.Register("basedir", (logEvent) => hostingContext.HostingEnvironment.ContentRootPath);
            LayoutRenderer.Register<AspNetBuildDateLayoutRenderer>("custom-build-date");
            //hostingContext.HostingEnvironment.ConfigureNLog("nlog.config");

            LogHolder.Init(new NLogFactory());
        }


        private static void ConfigConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;
            config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                config.AddUserSecrets<Startup>();
            }
        }
    }
}
