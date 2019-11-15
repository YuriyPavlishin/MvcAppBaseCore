using System;
using System.IO;
using BaseApp.Common.Logs;
using BaseApp.Web.Code.Infrastructure.Logs;
using BaseApp.Web.Code.Infrastructure.Logs.LogRenderers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.LayoutRenderers;
using BaseApp.Web.Code.Extensions;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using NLog.Web;

namespace BaseApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                var host = CreateHostBuilder(args).Build();
                    //.Migrate();

                host.Run();
            }
            catch (Exception ex)
            {
                new StartupLogger(currentDirectory).ErrorException(ex, "Main method");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder
                          .UseContentRoot(Directory.GetCurrentDirectory())
                          .ConfigureAppConfiguration(ConfigConfiguration)
                          .ConfigureLogging(ConfigureLogging)
                          .CaptureStartupErrors(true)
                          .UseNLog()
                          .UseIISIntegration()
                          .UseStartup<Startup>();
                  });

        // neccessary for EF Tooling
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    return new WebHostBuilder()
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .ConfigureAppConfiguration(ConfigConfiguration)
        //        .ConfigureLogging(ConfigureLogging)
        //        .CaptureStartupErrors(true)
        //        .UseNLog()
        //        .UseIISIntegration()
        //        .UseStartup<Startup>();
        //}

        private static void ConfigureLogging(WebHostBuilderContext hostingContext, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
            //AspNetExtensions.AddNLog(loggingBuilder, "nlog.config");

            LayoutRenderer.Register("basedir", (logEvent) => hostingContext.HostingEnvironment.ContentRootPath);
            LayoutRenderer.Register<AspNetBuildDateLayoutRenderer>("custom-build-date");

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
