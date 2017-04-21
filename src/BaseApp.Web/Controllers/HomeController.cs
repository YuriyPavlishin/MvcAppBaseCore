using BaseApp.Common.Logs;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class HomeController : ControllerBaseNoAuthorize
    {
        public IActionResult Index()
        {
            LogHolder.MainLog.Error("This is error text for log");
            LogHolder.MainLog.Info("Test this engine");
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
