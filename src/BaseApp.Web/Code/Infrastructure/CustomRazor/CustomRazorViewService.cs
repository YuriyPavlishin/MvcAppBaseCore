using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BaseApp.Web.Code.Infrastructure.CustomRazor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;

namespace BaseApp.Web.Code.Infrastructure.CustomRazor
{
    public class CustomRazorViewService: ICustomRazorViewService
    {
        private readonly CustomRazorViewEngine _engine;

        public CustomRazorViewService()
        {
            var serviceScope = CreateScopeFactoryForCustomRazor(Path.Combine(AppContext.BaseDirectory, "Views\\Templates\\"))
                .CreateScope();
            _engine = serviceScope.ServiceProvider.GetRequiredService<CustomRazorViewEngine>();
        }

        public string Render<T>(string viewPath, T model) => RenderAsync(viewPath, model).Result;
        public async Task<string> RenderAsync<T>(string viewPath, T model) => await _engine.RenderViewToStringAsync(viewPath, model);

        private static IServiceScopeFactory CreateScopeFactoryForCustomRazor(string pathToViewFolder)
        {
            var services = new ServiceCollection();

            var appName = Assembly.GetEntryAssembly().GetName().Name;
            var fileProvider = new PhysicalFileProvider(pathToViewFolder);
            var listener = new DiagnosticListener("Microsoft.AspNetCore");

            services.AddSingleton<IWebHostEnvironment>(new CustomRazorViewWebHostEnvironment
            {
                WebRootFileProvider = fileProvider,
                ContentRootFileProvider = fileProvider,
                ApplicationName = appName,
                ContentRootPath = pathToViewFolder,
                WebRootPath = pathToViewFolder
            });
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddLogging();
            services.AddControllersWithViews()
                .AddRazorOptions(o => o.ViewLocationFormats.Add("/{0}.cshtml"))
                .AddRazorRuntimeCompilation();
            services.AddTransient<CustomRazorViewEngine>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }
    }
}