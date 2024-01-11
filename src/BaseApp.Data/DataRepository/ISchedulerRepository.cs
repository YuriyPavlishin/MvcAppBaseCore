using System.Threading.Tasks;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository
{
    public interface ISchedulerRepository : IRepositoryEntityBase<Scheduler>
    {
        Task<Scheduler> GetNextSchedulerToProcessAsync();
        Task<NotificationEmail> GetNextEmailToProcessAsync(int? schedulerId = null, bool isSync = false);
        NotificationEmail GetNotificationEmail(int notificationEmailId);
        NotificationEmail CreateNotificationEmail();
    }
}
