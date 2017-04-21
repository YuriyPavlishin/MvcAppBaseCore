using BaseApp.Common.Extensions;
using BaseApp.Common.Logs;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;

namespace BaseApp.Web.Controllers
{
    public class ErrorsController : ControllerBaseNoAuthorize
    {
        public IActionResult Statuses(string id)
        {
            if (id.EqualsIgnoreCase("404"))
            {
                var requestFeature = HttpContext.Features.Get<IHttpRequestFeature>();
                LogHolder.Http404Log.Error(requestFeature.RawTarget);
            }
            
            var errorStatus = "The following error " + id + " occured";
            return View((object)errorStatus);
        }
    }
}
