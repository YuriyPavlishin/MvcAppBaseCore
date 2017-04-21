using System;

namespace BaseApp.Common.Logs
{
    public interface ILogFactory
    {
        ILogger GetLogger(string name);
    }

    public interface ILogger
    {
        void Error(Exception exception, string message = null);
        void Error(string message);
        void Info(string message);
    }
}
