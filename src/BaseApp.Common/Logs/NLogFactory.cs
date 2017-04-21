using System;
using NLog;

namespace BaseApp.Common.Logs
{
    public class NLogFactory : ILogFactory
    {
        public ILogger GetLogger(string name)
        {
            return new NLogger(LogManager.GetLogger(name));
        }
    }

    public class NLogger : ILogger
    {
        private readonly Logger _Logger;
        public NLogger(Logger logger)
        {
            _Logger = logger;
        }

        public void Error(Exception exception, string message)
        {
            _Logger.Error(exception, message);
        }

        public void Error(string message)
        {
            _Logger.Error(message);
        }

        public void Info(string message)
        {
            _Logger.Info(message);
        }
    }
}
