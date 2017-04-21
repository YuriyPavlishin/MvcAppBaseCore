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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Web.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppWeb(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.Configure<SiteOptions>(configurationRoot.GetSection("SiteOptions"));

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IPathResolver, PathResolver>();

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScoped<ILogonManager, LogonManager>();
            services.AddScoped<ILoggedUserAccessor, LoggedUserAccessor>();

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

        private static void AddFiles(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.Configure<FileFactoryOptions>(configurationRoot.GetSection("FileOptions"));
            services.AddSingleton<IFileFactoryService, FileFactoryService>();
            services.AddSingleton<IAttachmentService, AttachmentService>();
        }

        public static void AddAppWebSecurity(this IServiceCollection services, IHostingEnvironment env)
        {
            //RSAKeyUtils.GenerateKeyAndSave(env.ContentRootPath + "\\App_Data\\RSAkey.txt");
            RSAParameters keyParams = RSAKeyUtils.GetKeyParameters(env.ContentRootPath + "\\App_Data\\RSAkey.txt");
            var key = new RsaSecurityKey(keyParams);

            services.Configure<TokenAuthOptions>(tokenAuthOptions =>
            {
                tokenAuthOptions.Audience = "ExampleAudience";
                tokenAuthOptions.Issuer = "ExampleIssuer";
                tokenAuthOptions.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
                tokenAuthOptions.IssuerSigningKey = key;
            });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(ApiConstants.ApiPolicy, new AuthorizationPolicyBuilder()
                    //.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​, CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
    }
}
