using System;
using System.Reflection;
using System.Text;
using NLog;
using NLog.Web.LayoutRenderers;

namespace BaseApp.Web.Code.Infrastructure.Logs.LogRenderers
{
    public class AspNetBuildDateLayoutRenderer: AspNetLayoutRendererBase
    {
        private static DateTime? _cachedBuildDate;

        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            try
            {
                if (_cachedBuildDate == null)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    _cachedBuildDate = System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);
                }
                builder.Append(_cachedBuildDate.Value);
            }
            catch
            {
                // ignored
            }
        }
    }
}
