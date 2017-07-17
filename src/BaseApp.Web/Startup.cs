using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BaseApp.Common.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BaseApp.Data.DataContext;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.Api.Swagger;
using BaseApp.Web.Code.Infrastructure.Logs;
using BaseApp.Web.Code.Scheduler.Queue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostEnv;
        private readonly List<Exception> _startupExceptions = new List<Exception>();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
            _hostEnv = env;
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMemoryCache();
                services.AddOptions();

                services.AddDbContext<DBData>(options =>
                    {
                        options.UseSqlServer(
                            Configuration["Data:DefaultConnection:ConnectionString"]
                            , b => b.MigrationsAssembly("BaseApp.Data.ProjectMigration"));
                    }
                );

                services.AddAppWeb(Configuration);
                services.AddAppWebSecurity(_hostEnv);

                services
                    .AddMvc(options => { options.Conventions.Add(new ApiControllerConvention()); })
                    .AddJsonOptions(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });

                services.AddAppWebSwagger();
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                app.UseConfiguredLogs(env, loggerFactory, Configuration);
            }
            catch (Exception ex)
            {
                new StartupLogger(env.ContentRootPath).ErrorException(ex);
                _startupExceptions.Add(ex);
            }

            try
            {
                if (!_startupExceptions.Any())
                {
                    UseAppInner(app, env);
                }
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
                LogHolder.MainLog.Error(ex);
            }

            if (_startupExceptions.Any())
            {
                try
                {
                    _startupExceptions.ForEach(ex => LogHolder.MainLog.Error(ex));
                }
                catch (Exception ex)
                {
                    new StartupLogger(env.ContentRootPath).ErrorException(ex);
                }
                
                RenderStartupErrors(app);
            }
        }

        private static void UseAppInner(IApplicationBuilder app, IHostingEnvironment env)
        {
            AppDependencyResolver.Init(app.ApplicationServices);
            app.UseStatusCodePagesWithReExecute("/Errors/Statuses/{0}");

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors/Index");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<DBData>().Database.Migrate();
                    }
                }
                catch
                {
                }
            }

            app.ApplicationServices.GetRequiredService<WorkersQueue>().Init();

            app.UseStaticFiles();
            app.UseAppWebSecurity();
            app.UseMvc(routes =>
                       {
                           routes.MapRoute(name: "areaRoute",
                               template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                           routes.MapRoute(
                               name: "default",
                               template: "{controller=Home}/{action=Index}/{id?}");
                       });
            app.UseAppWebSwagger();
        }

        private void RenderStartupErrors(IApplicationBuilder app)
        {
            app.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";

                    foreach (var ex in _startupExceptions)
                    {
                        await context.Response.WriteAsync($"Error on startup {ex.Message}").ConfigureAwait(false);
                    }
                });
        }
    }
}
