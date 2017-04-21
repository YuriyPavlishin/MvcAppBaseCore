using BaseApp.Data.Files;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Templating;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions
{
    public class SchedulerActionArgs
    {
        public UnitOfWork UnitOfWork { get; set; }
        public IPathResolver PathResolver { get; set; }
        public ITemplateBuilder TemplateBuilder { get; set; }
        public IAttachmentService AttachmentService { get; set; }
    }
}