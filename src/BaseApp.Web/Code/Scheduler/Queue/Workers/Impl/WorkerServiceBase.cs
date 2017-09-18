using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers.Impl
{
    public abstract class WorkerServiceBase
    {
        public abstract void LoadAndProcess();

        protected IUnitOfWork CreateUnitOfWork()
        {
            return AppDependencyResolver.Current.CreateUoWinCurrentThread();
        }
    }
}