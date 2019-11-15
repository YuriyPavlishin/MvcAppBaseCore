using System;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using BaseApp.Common.Emails;
using BaseApp.Common.Emails.Impl;
using BaseApp.Common.Files;
using BaseApp.Common.Files.Impl;
using BaseApp.Common.Files.Models;
using BaseApp.Common.Utils.Email;
using BaseApp.Data.Files;
using BaseApp.Data.Files.Impl;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Menu;
using BaseApp.Web.Code.Infrastructure.Templating;
using BaseApp.Web.Code.Infrastructure.TokenAuth;
using BaseApp.Web.Code.Mappers;
using BaseApp.Web.Code.Scheduler;
using BaseApp.Web.Code.Scheduler.Queue;
using BaseApp.Web.Code.Scheduler.Queue.Workers;
using BaseApp.Web.Code.Scheduler.Queue.Workers.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BaseApp.Web.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppWeb(this IServiceCollection services, IConfiguration configurationRoot)
        {
            services.Configure<SiteOptions>(configurationRoot.GetSection("SiteOptions"));

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IPathResolver, PathResolver>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILoggedUserAccessor, LoggedUserAccessor>();
            services.AddScoped<ILogonManager, LogonManager>();

            services.AddScoped<IMenuBuilderFactory, MenuBuilderFactory>();
            services.AddScoped<ViewDataItems>();

            services.AddSingleton(sp => MapInit.CreateConfiguration().CreateMapper());

            AddFiles(services, configurationRoot);

            services.AddSingleton<ITemplateBuilder, TemplateBuilder>();
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.Configure<EmailSenderOptions>(configurationRoot.GetSection("EmailSenderOptions"));

            services.AddTransient<IEmailWorkerService, EmailWorkerService>();
            services.AddTransient<ISchedulerWorkerService, SchedulerWorkerService>();
            services.AddSingleton<WorkersQueue>();
            services.AddSingleton<ISchedulerService, SchedulerService>();
        }

        private static void AddFiles(IServiceCollection services, IConfiguration configurationRoot)
        {
            services.Configure<FileFactoryOptions>(configurationRoot.GetSection("FileOptions"));
            services.AddSingleton<IFileFactoryService, FileFactoryService>();
            services.AddSingleton<IAttachmentService, AttachmentService>();
        }

        public static void AddAppWebSecurity(this IServiceCollection services, IWebHostEnvironment env)
        {
            //RSAKeyUtils.GenerateKeyAndSave(env.ContentRootPath + "\\App_Data\\RSAkey.txt");
            RSAParameters keyParams = RSAKeyUtils.GetKeyParameters(env.ContentRootPath + "\\App_Data\\RSAkey.txt");
            var key = new RsaSecurityKey(keyParams);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(env.ContentRootPath + "\\App_Data\\PersistKeys\\"))
                .SetApplicationName("BaseApp");

            services.Configure<TokenAuthOptions>(tokenAuthOptions =>
                                                 {
                                                     tokenAuthOptions.Audience = GetAudience();
                                                     tokenAuthOptions.Issuer = GetIssuer();
                                                     tokenAuthOptions.SigningCredentials =
                                                         new SigningCredentials(key,
                                                             SecurityAlgorithms.RsaSha256Signature);
                                                     tokenAuthOptions.IssuerSigningKey = key;
                                                 });


            services.AddAuthorization(auth =>
                                      {
                                          auth.AddPolicy(ApiConstants.ApiPolicy, new AuthorizationPolicyBuilder()
                                              //.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​, CookieAuthenticationDefaults.AuthenticationScheme)
                                              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                              .RequireAuthenticatedUser()
                                              .Build());
                                      })
                .AddAuthentication(o =>
                                   {
                                       o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                                       o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                                   })
                .AddCookie(options =>
                           {
                               options.Cookie.Name = ".BaseApp.Web-Core-AUTH";
                               options.LoginPath = new PathString("/Account/LogOn/");
                               options.LogoutPath = new PathString("/Account/LogOff/");
                               options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                               options.ExpireTimeSpan = TimeSpan.FromDays(60);
                           })
                .AddJwtBearer(options =>
                              {
                                  options.TokenValidationParameters = new TokenValidationParameters()
                                                                      {
                                                                          ValidateIssuerSigningKey = true,
                                                                          IssuerSigningKey = key,
                                                                          ValidateIssuer = true,
                                                                          ValidIssuer = GetIssuer(),
                                                                          ValidateAudience = true,
                                                                          ValidAudience = GetAudience(),
                                                                          ValidateLifetime = true,
                                                                          // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                                                                          // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                                                                          // machines which should have synchronised time, this can be set to zero. Where external tokens are
                                                                          // used, some leeway here could be useful.
                                                                          ClockSkew = TimeSpan.FromMinutes(0)
                                                                      };

                                  options.RequireHttpsMetadata = false;
                              });
        }

        private static string GetIssuer()
        {
            return "ExampleIssuer";
        }

        private static string GetAudience()
        {
            return "ExampleAudience";
        }
    }
}