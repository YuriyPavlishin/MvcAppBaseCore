using System;

namespace BaseApp.Web.Code.Infrastructure
{
    public class StartupLogger
    {
        private static readonly object _locker = new object();

        private readonly string _BaseFolder;

        public StartupLogger(string baseFolder)
        {
            _BaseFolder = baseFolder;
        }

        public void ErrorException(Exception exception, string message = null)
        {
            lock (_locker)
            {
                try
                {
                    var text = message ?? "";
                    text += "\r\n" + exception;

                    System.IO.File.AppendAllText($"{_BaseFolder}/App_Data/Logs/Startup.txt",
                        $"{DateTime.Now}: {text}\r\n\r\n");
                }
                catch
                {
                }
            }
        }
    }
}
