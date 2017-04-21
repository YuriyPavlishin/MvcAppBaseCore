using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BaseApp.Common.Log;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BaseApp.Data.DataContext;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Api.Swagger;
using BaseApp.Web.Code.Scheduler.Queue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Web;
using Swashbuckle.Swagger.Model;

namespace BaseApp.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _HostEnv;
        private readonly List<Exception> _StartupExceptions = new List<Exception>();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _HostEnv = env;
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
                services.AddAppWebSecurity(_HostEnv);

                services.AddMvc()
                    .AddMvcOptions(options => { options.Filters.Add(new GlobalExceptionFilter()); })
                    .AddJsonOptions(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });

                services.AddSwaggerGen(options =>
                {
                    options.DescribeAllEnumsAsStrings();
                    options.SingleApiVersion(new Info
                    {
                        Version = "v1", Title = "BaseApp", Description = "Api Help Page", TermsOfService = "None",
                    });
                    options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                    options.OperationFilter<QueryModelBindingOperationFilter>();
                    options.OperationFilter<FormFileOperationFilter>();
                    
                    //Determine base path for the application.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    //Set the comments path for the swagger json and ui.
                    options.IncludeXmlComments(basePath + "\\BaseApp.Web.xml");

                });
            }
            catch (Exception ex)
            {
                _StartupExceptions.Add(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                ConfigureLogs(app, env, loggerFactory);
            }
            catch (Exception ex)
            {
                new StartupLogger(env.ContentRootPath).ErrorException(ex);
                _StartupExceptions.Add(ex);
            }

            try
            {
                if (!_StartupExceptions.Any())
                {
                    ConfigureAppInner(app, env);
                }
            }
            catch (Exception ex)
            {
                _StartupExceptions.Add(ex);
                LogHolder.MainLog.Error(ex);
            }

            if (_StartupExceptions.Any())
            {
                try
                {
                    _StartupExceptions.ForEach(ex => LogHolder.MainLog.Error(ex));
                }
                catch (Exception ex)
                {
                    new StartupLogger(env.ContentRootPath).ErrorException(ex);
                }
                
                RenderStartupErrors(app);
            }
        }

        private void ConfigureLogs(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            LayoutRenderer.Register("basedir", (logEvent) => env.ContentRootPath);
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");

            LogHolder.Init(new NLogFactory());
        }

        private static void ConfigureAppInner(IApplicationBuilder app, IHostingEnvironment env)
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
                app.UseExceptionHandler("/Home/Error");

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

            app.UseStaticFiles();

            app.ApplicationServices.GetRequiredService<WorkersQueue>().Init();
            app.UseAppWebSecurity();

            app.UseMvc(routes =>
                       {
                           routes.MapRoute(name: "areaRoute",
                               template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                           routes.MapRoute(
                               name: "default",
                               template: "{controller=Home}/{action=Index}/{id?}");
                       });

            app.UseSwagger();
            app.UseSwaggerUi();
        }

        private void RenderStartupErrors(IApplicationBuilder app)
        {
            app.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";

                    foreach (var ex in _StartupExceptions)
                    {
                        await context.Response.WriteAsync($"Error on startup {ex.Message}").ConfigureAwait(false);
                    }
                });
        }
    }
}
