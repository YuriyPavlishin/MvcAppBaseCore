using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Web.Code.Infrastructure;
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
