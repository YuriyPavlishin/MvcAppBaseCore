using BaseApp.Web.Code.Infrastructure.BaseControllers;
using BaseApp.Web.Components.Example;
using BaseApp.Web.Models.Example;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class ExampleController : ControllerBaseAuthorizeRequired
    {
        public IActionResult Countries()
        {
            return View(new CountryArgsModel());
        }
        
        public IActionResult GetCountriesList(CountryArgsModel args)
        {
            return ViewComponent(typeof (CountryListViewComponent), new {args});
        }

        public IActionResult ApiUpload()
        {
            return View();
        }
    }
}
