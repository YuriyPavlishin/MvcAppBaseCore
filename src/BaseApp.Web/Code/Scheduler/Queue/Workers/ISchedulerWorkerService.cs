using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    public interface ISchedulerWorkerService : IWorkerServiceBase
    {
        void ProcessSchedulerSync(SchedulerData schedulerData);
    }
}
