using System.Threading.Tasks;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    public interface IWorkerServiceBase
    {
        Task LoadAndProcessAsync();
        void WakeUp();
        void Delay();
    }
}
