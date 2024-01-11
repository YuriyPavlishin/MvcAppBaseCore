using System;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using BaseApp.Common.Files.Models;
using BaseApp.Common.Utils.Email;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.TokenAuth;
using BaseApp.Web.Code.Scheduler.Queue;
using BaseApp.Web.Code.Scheduler.Queue.Workers;
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
            services.Configure<FileFactoryOptions>(configurationRoot.GetSection("FileOptions"));
            services.Configure<EmailSenderOptions>(configurationRoot.GetSection("EmailSenderOptions"));

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddHostedService<SchedulerHostedService<IEmailWorkerService>>();
            services.AddHostedService<SchedulerHostedService<ISchedulerWorkerService>>();
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