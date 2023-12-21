using System;
using System.Collections.Generic;
using System.Linq;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DataRepository.Impl
{
    public class SchedulerRepository(DataContextProvider context) : RepositoryEntityBase<Scheduler>(context), ISchedulerRepository
    {
        public Scheduler GetScheduler(int schedulerId)
        {
            return Context.Set<Scheduler>().FirstOrDefault(m => m.Id == schedulerId);
        }

        public NotificationEmail GetNotificationEmail(int notificationEmailId)
        {
            return Context.Set<NotificationEmail>().FirstOrDefault(m => m.Id == notificationEmailId);
        }

        public List<Scheduler> GetSchedulersToProcess()
        {
            var currentdate = DateTime.Now;

            return Context.Set<Scheduler>()
                .Where(m => m.StartProcessDate == null)
                .Where(m => m.OnDate <= currentdate)
                .Where(m => !m.IsSynchronous)
                .OrderBy(m => m.CreatedDate)
                .ToList();
        }

        public List<NotificationEmail> GetEmailsToProcess(int? schedulerId = null, bool isSync = false)
        {
            var q = Context.Set<NotificationEmail>()
                .Where(m => m.ProcessedDate == null)
                .Where(m => m.Scheduler.IsSynchronous == isSync);

            if (schedulerId != null)
            {
                q = q.Where(m => m.SchedulerId == schedulerId);
            }

            return q
                .OrderBy(m => m.CreatedDate)
                .Include(m => m.Scheduler)
                .Include(m => m.NotificationEmailAttachments).ThenInclude(m => m.Attachment)
                .ToList();
        }

        public NotificationEmail CreateNotificationEmail()
        {
            return CreateEmpty<NotificationEmail>();
        }
    }
}
