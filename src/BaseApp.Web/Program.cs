using System;
using System.IO;
using BaseApp.Web.Code.Infrastructure.Logs;
using Microsoft.AspNetCore.Hosting;

namespace BaseApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            try
            {
                var host = new WebHostBuilder()
                  .UseKestrel()
                  .UseContentRoot(currentDirectory)
                  .UseIISIntegration()
                  .UseStartup<Startup>()
                  .Build();
                
                host.Run();
            }
            catch (Exception ex)
            {
                new StartupLogger(currentDirectory).ErrorException(ex, "Main method");
                throw;
            }
        }
    }
}
