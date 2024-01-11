using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.Scheduler.SchedulerModels;

namespace BaseApp.Web.Code.Scheduler
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface ISchedulerService
    {
        void ScheduleAction<T>(T schedulerModel) where T : SchedulerModelBase;
        Task EmailSynchronizedAsync<T>(T schedulerModel) where T : SchedulerModelBase;
    }
}
