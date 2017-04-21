using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseApp.Common;
using BaseApp.Web.Code.Scheduler.Attributes;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerModels.GenericActionModels
{
    [SchedulerActionType(Enums.SchedulerActionTypes.ExampleAction)]
    public class ExampleActionModel : SchedulerModelBase
    {
        public string Value { get; set; }

        public ExampleActionModel(int createdByUserId) : base(createdByUserId)
        {
        }

        protected override void DoFillFromSchedulerData(SchedulerData schedulerData)
        {
            Value = schedulerData.EntityData1;
        }

        protected override void DoFillSchedulerData(SchedulerData schedulerData)
        {
            schedulerData.EntityData1 = Value;
        }
    }
}