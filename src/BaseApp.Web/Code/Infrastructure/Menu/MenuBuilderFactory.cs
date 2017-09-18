using System;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Menu.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BaseApp.Web.Code.Infrastructure.Menu
{
    public class MenuBuilderFactory: IMenuBuilderFactory
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUserAccessor _loggedUserAccessor;

        public MenuBuilderFactory(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory
            , IUnitOfWork unitOfWork, ILoggedUserAccessor loggedUserAccessor)
        {
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _unitOfWork = unitOfWork;
            _loggedUserAccessor = loggedUserAccessor;
        }

        public IMenuBuilder Create<T>() where T: MenuBuilderBase
        {
            var mvcArgs = new MenuMvcArgs(_actionContextAccessor, _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext));
            var args = new MenuBuilderArgs(mvcArgs, _unitOfWork, _loggedUserAccessor);

            return (IMenuBuilder)Activator.CreateInstance(typeof(T), args);            
        }

    }
}
