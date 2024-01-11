using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Exceptions;
using BaseApp.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DataRepository.Impl
{
    public class SchedulerRepository(DataContextProvider context) : RepositoryEntityBase<Scheduler>(context), ISchedulerRepository
    {
        public async Task<Scheduler> GetNextSchedulerToProcessAsync()
        {
            var currentDate = DateTime.Now;

            return await Context.Set<Scheduler>()
                .Where(m => m.StartProcessDate == null)
                .Where(m => m.OnDate <= currentDate)
                .Where(m => !m.IsSynchronous)
                .OrderBy(m => m.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<NotificationEmail> GetNextEmailToProcessAsync(int? schedulerId = null, bool isSync = false)
        {
            var q = Context.Set<NotificationEmail>()
                .Where(m => m.ProcessedDate == null)
                .Where(m => m.Scheduler.IsSynchronous == isSync);

            if (schedulerId != null)
            {
                q = q.Where(m => m.SchedulerId == schedulerId);
            }

            return await q
                .OrderBy(m => m.CreatedDate)
                .Include(m => m.Scheduler)
                .Include(m => m.NotificationEmailAttachments).ThenInclude(m => m.Attachment)
                .FirstOrDefaultAsync();
        }
        
        public NotificationEmail GetNotificationEmail(int notificationEmailId)
        {
            return Context.Set<NotificationEmail>().Find(notificationEmailId) 
                   ?? throw new RecordNotFoundException(typeof(NotificationEmail), notificationEmailId);
        }

        public NotificationEmail CreateNotificationEmail()
        {
            return CreateEmpty<NotificationEmail>();
        }
    }
}
