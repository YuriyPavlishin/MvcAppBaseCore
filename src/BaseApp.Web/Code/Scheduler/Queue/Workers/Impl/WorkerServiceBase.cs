using System;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers.Impl
{
    public abstract class WorkerServiceBase(Func<IUnitOfWorkPerCall> unitOfWorkPerCallFunc)
    {
        public abstract void LoadAndProcess();

        protected IUnitOfWork CreateUnitOfWork()
        {
            return unitOfWorkPerCallFunc();
        }
    }
}