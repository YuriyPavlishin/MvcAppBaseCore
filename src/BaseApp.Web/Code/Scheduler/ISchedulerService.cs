using BaseApp.Web.Code.Scheduler.SchedulerModels;

namespace BaseApp.Web.Code.Scheduler
{
    public interface ISchedulerService
    {
        void ScheduleAction<T>(T schedulerModel) where T : SchedulerModelBase;
        void EmailSync<T>(T schedulerModel) where T : SchedulerModelBase;
    }
}
