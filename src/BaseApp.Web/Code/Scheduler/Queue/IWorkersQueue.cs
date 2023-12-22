using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.Queue;

[Injectable(InjectableTypes.SingleInstance)]
public interface IWorkersQueue
{
    void Init();
    void EmailSync(SchedulerData schedulerData);
    void WakeUpScheduler();
}