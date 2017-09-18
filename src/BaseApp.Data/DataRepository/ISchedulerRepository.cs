using System.Collections.Generic;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository
{
    public interface ISchedulerRepository : IRepositoryEntityBase<Scheduler>
    {
        Scheduler GetScheduler(int schedulerId);
        NotificationEmail GetNotificationEmail(int notificationEmailId);
        List<Scheduler> GetSchedulersToProcess();
        List<NotificationEmail> GetEmailsToProcess(int? schedulerId = null, bool isSync = false);
        NotificationEmail CreateNotificationEmail();
    }
}
