using System;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Scheduler.DataModels;
using BaseApp.Web.Code.Scheduler.SchedulerModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions
{
    public abstract class SchedulerActionBase<T> : ISchedulerAction
        where T : SchedulerModelBase
    {
        protected UnitOfWork UnitOfWork { get; private set; }
        protected SchedulerActionArgs ActionArgs { get; private set; }
        
        protected SchedulerActionBase(SchedulerActionArgs args)
        {
            UnitOfWork = args.UnitOfWork;
            ActionArgs = args;
        }

        public void Process(SchedulerData schedulerData)
        {
            var model = GetModel(schedulerData);
            DoProcess(model);
        }

        protected abstract void DoProcess(T actionModel);

        private T GetModel(SchedulerData schedulerData)
        {
            var model = (T)Activator.CreateInstance(typeof(T), schedulerData.CreatedByUserId);
            model.FillFromSchedulerData(schedulerData);

            return model;
        }
    }
}