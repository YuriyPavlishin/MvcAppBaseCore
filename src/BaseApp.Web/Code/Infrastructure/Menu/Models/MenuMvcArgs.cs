using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace BaseApp.Web.Code.Infrastructure.Menu.Models
{
    public class MenuMvcArgs
    {
        private IActionContextAccessor ActionContextAccessor { get; }

        public IUrlHelper UrlHelper { get; }
        public RouteData RouteData => ActionContextAccessor.ActionContext.RouteData;
        public IQueryCollection QueryString => ActionContextAccessor.ActionContext.HttpContext.Request.Query;

        public MenuMvcArgs(IActionContextAccessor actionContextAccessor, IUrlHelper urlHelper)
        {
            ActionContextAccessor = actionContextAccessor;
            UrlHelper = urlHelper;
        }
    }
}
