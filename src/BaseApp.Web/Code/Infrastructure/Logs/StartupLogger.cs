using System;

namespace BaseApp.Web.Code.Infrastructure.Logs
{
    public class StartupLogger
    {
        private static readonly object Locker = new object();

        private readonly string _baseFolder;

        public StartupLogger(string baseFolder)
        {
            _baseFolder = baseFolder;
        }

        public void ErrorException(Exception exception, string message = null)
        {
            lock (Locker)
            {
                try
                {
                    var text = message ?? "";
                    text += "\r\n" + exception;

                    System.IO.File.AppendAllText($"{_baseFolder}/App_Data/Logs/Startup.txt",
                        $"{DateTime.Now}: {text}\r\n\r\n");
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }
        }
    }
}
