using BaseApp.Common.Injection.Config;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    [Injectable(InjectableTypes.Dependency)]
    public interface IEmailWorkerService: IWorkerServiceBase
    {
        void ProcessSchedulerSync(int schedulerId);
    }
}
