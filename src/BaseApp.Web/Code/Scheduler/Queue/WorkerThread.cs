using System;
using System.Threading;
using BaseApp.Common;
using BaseApp.Common.Log;
using BaseApp.Web.Code.Scheduler.Queue.Workers;

namespace BaseApp.Web.Code.Scheduler.Queue
{
    public class WorkerThread<T> where T : IWorkerServiceBase
    {
        private AutoResetEvent ResetEvent { get; set; }
        private Thread WorkingThread { get; set; }
        public T NotificationWorker { get; private set; }

        private int DelayMilliseconds { get; set; }

        private bool WakeUpCalled { get; set; }

        public WorkerThread(string threadName, T notificationWorker)
        {
            DelayMilliseconds = 5*60*1000;

            NotificationWorker = notificationWorker;

            ResetEvent = new AutoResetEvent(false);

            WorkingThread = new Thread(ProcessWorker)
            {
                IsBackground = true,
                Name = threadName
            };
            WorkingThread.Start();
        }

        public void WakeUp()
        {
            WakeUpCalled = true;
            ResetEvent.Set();
        }

        private void ProcessWorker()
        {
            while (true)
            {
                try
                {
                    NotificationWorker.LoadAndProcess();
                }
                catch (Exception ex)
                {
                    LogHolder.MainLog.Error(ex, "Error occured in notification worker - " + NotificationWorker.GetType());
                }

                if (!WakeUpCalled)
                {
                    ResetEvent.WaitOne(DelayMilliseconds);
                }

                WakeUpCalled = false;
            }
        }
    }
}