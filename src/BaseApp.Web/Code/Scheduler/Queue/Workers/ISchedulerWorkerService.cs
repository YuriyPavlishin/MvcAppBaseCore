using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface ISchedulerWorkerService : IWorkerServiceBase
    {
        Task ProcessSchedulerSynchronizedAsync(SchedulerData schedulerData);
    }
}
