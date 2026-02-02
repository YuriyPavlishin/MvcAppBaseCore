using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BaseApp.Web.Code.Infrastructure.Menu.Models
{
    public class MenuMvcArgs
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public IUrlHelper UrlHelper { get; }
        public RouteData RouteData => HttpContextAccessor.HttpContext?.GetRouteData();
        public IQueryCollection QueryString => HttpContextAccessor.HttpContext?.Request.Query;

        public MenuMvcArgs(IHttpContextAccessor httpContextAccessor, IUrlHelper urlHelper)
        {
            HttpContextAccessor = httpContextAccessor;
            UrlHelper = urlHelper;
        }
    }
}
