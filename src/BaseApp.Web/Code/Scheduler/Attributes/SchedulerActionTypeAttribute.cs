using System;
using BaseApp.Common;

namespace BaseApp.Web.Code.Scheduler.Attributes
{
    public class SchedulerActionTypeAttribute : Attribute
    {
        public Enums.SchedulerActionTypes SchedulerActionsType { get; private set; }

        public SchedulerActionTypeAttribute(Enums.SchedulerActionTypes schedulerActionsType)
        {
            SchedulerActionsType = schedulerActionsType;
        }
    }
}