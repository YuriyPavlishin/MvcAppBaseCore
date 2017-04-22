using System.Text;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;

namespace BaseApp.Web.Code.Infrastructure.Logs.LogRenderers
{
    public class AspNetRequestIpLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var httpContext = HttpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return;
            }
            builder.Append(httpContext.Connection.RemoteIpAddress);
        }
    }
}
