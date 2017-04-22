﻿using BaseApp.Common.Logs;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class HomeController : ControllerBaseNoAuthorize
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
