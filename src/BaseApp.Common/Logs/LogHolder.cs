using System;

namespace BaseApp.Common.Logs
{
    public static class LogHolder
    {
        public static void Init(ILogFactory logFactory)
        {
            if (logFactory == null)
                throw new ArgumentNullException(nameof(logFactory));

            MainLog = logFactory.GetLogger("MainLog");
            Http404Log = logFactory.GetLogger("Http404Log");
        }

        public static ILogger MainLog { get; private set; }
        public static ILogger Http404Log { get; private set; }
    }
}
