using System;
using System.Threading.Tasks;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using BaseApp.Web.Models.ForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class ForgotPasswordController(IForgotPasswordCommandManager commandManager) : ControllerBaseAuthorizeRequired
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(ForgotPasswordModel model)
        {
            if (await ValidateAndPerform(
                    async () => await commandManager.RequestAsync(new RequestForgotPasswordArgs {Email = model.Email}))
               )
            {
                return RedirectToAction("Success");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Success()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CompleteResetPassword(Guid id, [FromServices] IForgotPasswordQueryManager queryManager)
        {
            var data = queryManager.GetRequest(new GetRequestForgotPasswordArgs { RequestID = id });
            if (!string.IsNullOrEmpty(data.ErrorMessage))
            {
                return View("CompleteResetPasswordError", data.ErrorMessage);
            }
            var model = new CompleteResetPasswordModel { RequestId = id };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CompleteResetPassword(CompleteResetPasswordModel model)
        {
            var (isValid, result) = await ValidateAndPerform(async () => await commandManager.CompleteAsync(Mapper.Map<CompleteForgotPasswordArgs>(model)));
            if (isValid)
            {
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return View("CompleteResetPasswordError", result.ErrorMessage);
                }
                return RedirectToAction("CompleteResetPasswordSuccess");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult CompleteResetPasswordSuccess()
        {
            return View();
        }
    }
}
