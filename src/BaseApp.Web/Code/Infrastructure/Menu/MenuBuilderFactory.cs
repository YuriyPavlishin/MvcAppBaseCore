using System;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Menu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace BaseApp.Web.Code.Infrastructure.Menu
{
    public class MenuBuilderFactory: IMenuBuilderFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUserAccessor _loggedUserAccessor;

        public MenuBuilderFactory(IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory
            , IUnitOfWork unitOfWork, ILoggedUserAccessor loggedUserAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _unitOfWork = unitOfWork;
            _loggedUserAccessor = loggedUserAccessor;
        }

        public IMenuBuilder Create<T>() where T: MenuBuilderBase
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());

            var mvcArgs = new MenuMvcArgs(_httpContextAccessor, _urlHelperFactory.GetUrlHelper(actionContext));
            var args = new MenuBuilderArgs(mvcArgs, _unitOfWork, _loggedUserAccessor);

            return (IMenuBuilder)Activator.CreateInstance(typeof(T), args);
        }

    }
}
