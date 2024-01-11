using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface IEmailWorkerService: IWorkerServiceBase
    {
        Task ProcessSchedulerSynchronizedAsync(int schedulerId);
    }
}
