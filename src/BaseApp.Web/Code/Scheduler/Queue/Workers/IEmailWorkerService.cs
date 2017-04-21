namespace BaseApp.Web.Code.Scheduler.Queue.Workers
{
    public interface IEmailWorkerService: IWorkerServiceBase
    {
        void ProcessSchedulerSync(int schedulerId);
    }
}
