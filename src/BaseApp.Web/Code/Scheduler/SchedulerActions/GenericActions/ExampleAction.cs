using BaseApp.Common.Logs;
using BaseApp.Web.Code.Scheduler.SchedulerModels.GenericActionModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions.GenericActions
{
    public class ExampleAction : SchedulerActionBase<ExampleActionModel>
    {
        public ExampleAction(SchedulerActionArgs args) : base(args)
        {
        }

        protected override void DoProcess(ExampleActionModel actionModel)
        {
            LogHolder.MainLog.Info("Example Action fired - " + actionModel.Value);
        }
    }
}