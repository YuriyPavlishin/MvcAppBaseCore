using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions
{
    public interface ISchedulerAction
    {
        void Process(SchedulerData schedulerData);
    }
}