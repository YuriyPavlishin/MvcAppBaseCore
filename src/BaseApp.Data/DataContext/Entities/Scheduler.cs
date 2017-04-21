using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Common;

namespace BaseApp.Data.DataContext.Entities
{
    public class Scheduler
    {
        public Scheduler()
        {
            ChildSchedulers = new HashSet<Scheduler>();
            NotificationEmails = new HashSet<NotificationEmail>();
        }

        public int Id { get; set; }
        public Enums.SchedulerActionTypes SchedulerActionType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int? ParentSchedulerId { get; set; }
        public DateTime OnDate { get; set; }
        public bool IsSynchronous { get; set; }
        public DateTime? StartProcessDate { get; set; }
        public DateTime? EndProcessDate { get; set; }
        
        public string ErrorMessage { get; set; }
        public int? EntityId1 { get; set; }
        public int? EntityId2 { get; set; }
        public int? EntityId3 { get; set; }
        public int? EntityId4 { get; set; }

        
        public string EntityData1 { get; set; }
        public string EntityData2 { get; set; }
        public string EntityData3 { get; set; }
        public string EntityData4 { get; set; }
        

        public virtual User CreatedByUser { get; set; }
        public virtual Scheduler ParentScheduler { get; set; }
        public virtual ICollection<Scheduler> ChildSchedulers { get; set; }

        public virtual ICollection<NotificationEmail> NotificationEmails { get; set; }
    }
}
