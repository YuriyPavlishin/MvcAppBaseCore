using System.Threading.Tasks;
using BaseApp.Common.Logs;
using BaseApp.Web.Code.Scheduler.SchedulerModels.GenericActionModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions.GenericActions
{
    public class ExampleAction(SchedulerActionArgs args) : SchedulerActionBase<ExampleActionModel>(args)
    {
        protected override Task DoProcessAsync(ExampleActionModel actionModel)
        {
            LogHolder.MainLog.Info("Example Action fired - " + actionModel.Value);
            return Task.CompletedTask;
        }
    }
}