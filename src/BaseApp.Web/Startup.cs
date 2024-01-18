using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BaseApp.Common.Injection.Config;
using BaseApp.Common.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BaseApp.Data.DataContext;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.Api.Swagger;
using BaseApp.Web.Code.Infrastructure.Injection;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Logs;
using BaseApp.Web.Code.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BaseApp.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostEnv;
        private readonly List<Exception> _startupExceptions = new List<Exception>();
        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _hostEnv = env;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMemoryCache();
                services.AddOptions();

                services.AddDbContext<DBData>(options =>
                    {
                        options.UseLazyLoadingProxies().UseSqlServer(
                            Configuration["Data:DefaultConnection:ConnectionString"]
                            , b => b.MigrationsAssembly("BaseApp.Data.ProjectMigration"));
                    }
                );
                
                services.AddAppWeb(Configuration);
                services.AddAppWebSecurity(_hostEnv);

                services
                    .AddControllersWithViews(options => { options.Conventions.Add(new ApiControllerConvention()); })                    
                    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

                services.AddAppWebSwagger();
            }
            catch (Exception ex)
            {
                _startupExceptions.Add(ex);
            }
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(_ => MapInit.CreateConfiguration().CreateMapper()).SingleInstance();
            builder.RegisterType<ViewDataItems>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register(c =>
            {
                var scopeResolver = c.Resolve<IServiceScopeFactory>().CreateScope();
                return UnitOfWork.CreateInScope(scopeResolver.ServiceProvider.GetRequiredService<DBData>(), scopeResolver);
            }).As<IUnitOfWorkPerCall>().InstancePerDependency();
            InjectableRegistrationScanner.RegisterServices(builder, Assembly.GetAssembly(typeof(DBData)), Assembly.GetAssembly(typeof(InjectableAttribute)));
            InjectableRegistrationScanner.RegisterValidators(builder, Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
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

        private void UseAppInner(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
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
            }

            app.UseMiddleware<AjaxExceptionHandlerMiddleware>();

            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();//todo: check it
            
            app.Use(async (context, next) =>
            {
                var claims = context.RequestServices.GetRequiredService<ILogonManager>().LoggedClaims;
                context.RequestServices.GetRequiredService<ILoggedUserAccessor>().SetLoggedUser(claims);
                await next.Invoke();
            });

            app.UseEndpoints(routes =>
               {
                   routes.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                   routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
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
