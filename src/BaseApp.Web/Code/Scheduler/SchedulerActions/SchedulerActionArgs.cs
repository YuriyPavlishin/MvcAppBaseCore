using BaseApp.Data.Files;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.CustomRazor;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions
{
    public class SchedulerActionArgs
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IPathResolver PathResolver { get; set; }
        public ICustomRazorViewService CustomRazorViewService { get; set; }
        public IAttachmentService AttachmentService { get; set; }
    }
}