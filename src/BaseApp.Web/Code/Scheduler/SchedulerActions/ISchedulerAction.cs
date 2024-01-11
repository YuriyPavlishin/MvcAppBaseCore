using System.Threading.Tasks;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions
{
    public interface ISchedulerAction
    {
        Task ProcessAsync(SchedulerData schedulerData);
    }
}