using System;
using System.Threading;
using System.Threading.Tasks;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers.Impl
{
    public abstract class WorkerServiceBase(Func<IUnitOfWorkPerCall> unitOfWorkPerCallFunc)
    {
        private const int DelayMilliseconds = 5 * 60 * 1000;
        private AutoResetEvent ResetEvent { get; } = new(false);
        private bool WakeUpCalled { get; set; }

        public abstract Task LoadAndProcessAsync();
        
        public void WakeUp()
        {
            WakeUpCalled = true;
            ResetEvent.Set();
        }

        public void Delay()
        {
            if (!WakeUpCalled)
            {
                ResetEvent.WaitOne(DelayMilliseconds);
            }

            WakeUpCalled = false;
        }

        protected IUnitOfWork CreateUnitOfWork()
        {
            return unitOfWorkPerCallFunc();
        }
    }
}