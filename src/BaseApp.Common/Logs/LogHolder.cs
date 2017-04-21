using System;
using System.Collections.Generic;

namespace BaseApp.Common.Logs
{
    public static class LogHolder
    {
        private static readonly object locker = new object();

        private static ILogFactory _LogFactory;
        private static ILogFactory LogFactory
        {
            get
            {
                if (_LogFactory == null)
                    throw new Exception("logFactory not initialized");
                return _LogFactory;
            }
            set { _LogFactory = value; }
        }
        public static void Init(ILogFactory logFactory)
        {
            if (logFactory == null)
                throw new ArgumentNullException(nameof(logFactory));
            LogFactory = logFactory;
        }

        public static ILogger MainLog => GetLogger("MainLog");
        public static ILogger Http404Log => GetLogger("Http404Log");


        private static readonly Dictionary<string, ILogger> _Loggers = new Dictionary<string, ILogger>();
        private static ILogger GetLogger(string name)
        {
            lock (locker)
            {
                ILogger result;
                if (!_Loggers.TryGetValue(name, out result))
                {
                    result = LogFactory.GetLogger(name);
                    _Loggers.Add(name, result);
                }

                return result;
            }
        }
    }
}
