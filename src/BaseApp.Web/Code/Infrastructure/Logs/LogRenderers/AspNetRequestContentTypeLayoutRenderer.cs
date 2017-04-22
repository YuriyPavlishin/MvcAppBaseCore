using System.Text;
using NLog;
using NLog.Web.LayoutRenderers;

namespace BaseApp.Web.Code.Infrastructure.Logs.LogRenderers
{
    public class AspNetRequestContentTypeLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var contentType = HttpContextAccessor?.HttpContext?.Request?.ContentType;
            if (!string.IsNullOrEmpty(contentType))
                builder.Append(contentType);
        }
    }
}
