using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    [Injectable(InjectableTypes.Dependency)]
    public interface ISchedulerWorkerService : IWorkerServiceBase
    {
        void ProcessSchedulerSync(SchedulerData schedulerData);
    }
}
