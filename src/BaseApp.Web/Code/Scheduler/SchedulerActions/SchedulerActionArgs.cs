using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Injection;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions;

public class SchedulerActionArgs
{
    public IUnitOfWork UnitOfWork { get; init; }
    public IAppScope Scope { get; init; }
}