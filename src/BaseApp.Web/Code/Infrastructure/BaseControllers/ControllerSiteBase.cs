using AutoMapper;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Web.Code.Infrastructure.BaseControllers
{
    public abstract class ControllerSiteBase: Controller
    {
        protected IUnitOfWork UnitOfWork => HttpContext.RequestServices.GetRequiredService<IUnitOfWorkFactory>().UnitOfWork;
        protected ILoggedUserAccessor LoggedUser => HttpContext.RequestServices.GetRequiredService<ILoggedUserAccessor>();
        protected IMapper Mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}
