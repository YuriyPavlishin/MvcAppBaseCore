using BaseApp.Common.Logs;
using BaseApp.Web.Code.Infrastructure.Logs.LogRenderers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Web;

namespace BaseApp.Web.Code.Infrastructure.Logs
{
    public static class LogsExtensions
    {
        public static void UseConfiguredLogs(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfigurationRoot configurationRoot)
        {
            loggerFactory.AddConsole(configurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            LayoutRenderer.Register("basedir", (logEvent) => env.ContentRootPath);
            LayoutRenderer.Register<AspNetRequestIpLayoutRenderer>("custom-aspnet-request-ip");
            LayoutRenderer.Register<AspNetRequestContentTypeLayoutRenderer>("custom-aspnet-request-contenttype");
            LayoutRenderer.Register<AspNetBuildDateLayoutRenderer>("custom-build-date");
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");

            LogHolder.Init(new NLogFactory());
        }
    }
}
