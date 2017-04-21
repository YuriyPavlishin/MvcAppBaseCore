using System;
using BaseApp.Common;

namespace BaseApp.Web.Code.Scheduler.DataModels
{
    public class SchedulerData
    {
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }
        public Enums.SchedulerActionTypes SchedulerActionType { get; set; }
        public DateTime OnDate { get; set; }
        public int? EntityId1 { get; set; }
        public int? EntityId2 { get; set; }
        public int? EntityId3 { get; set; }
        public int? EntityId4 { get; set; }
        public string EntityData1 { get; set; }
        public string EntityData2 { get; set; }
        public string EntityData3 { get; set; }
        public string EntityData4 { get; set; }
    }
}