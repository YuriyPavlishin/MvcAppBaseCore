using System;
using BaseApp.Common.Extensions;
using BaseApp.Common.Utils;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class AccountController : ControllerBaseAuthorizeRequired
    {
        private readonly ILogonManager _logonManager;

        public AccountController(ILogonManager logonManager)
        {
            _logonManager = logonManager;
        }


        [AllowAnonymous]
        public IActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var account = model.AccountAfterValidation;
            _logonManager.SignInViaCookies(new LoggedClaims(account), model.RememberMe);

            return Redirect(Url.Home());
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var model = Mapper.Map<UserProfileModel>(
                UnitOfWork.Users.GetWithRolesOrNull(LoggedUser.Id)
            );

            return View(model);
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfileModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_Profile", model);

            var user = UnitOfWork.Users.GetWithRolesOrNull(LoggedUser.Id);

            var refreshClaims = !user.Login.EqualsIgnoreCase(model.Login);

            Mapper.Map(model, user);
            user.UpdatedByUserId = LoggedUser.Id;
            user.UpdatedDate = DateTime.Now;

            UnitOfWork.SaveChanges();
            _logonManager.RefreshCurrentLoggedUserInfo(refreshClaims);
                
            ClientMessage.AddSuccess("Profile was successfully updated.");
            return RedirectToAction("UserProfile");
        }

        public ActionResult _ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = UnitOfWork.Users.GetWithRolesOrNull(LoggedUser.Id);

            user.Password = PasswordHash.HashPassword(model.NewPassword);
            user.UpdatedDate = DateTime.Now;
            user.UpdatedByUserId = LoggedUser.Id;

            UnitOfWork.SaveChanges();

            return CloseDialog(new CloseDialogArgs() { SuccessMessage = "Your password has been changed successfully." });
        }

        public IActionResult LogOff()
        {
            _logonManager.SignOutAsCookies();

            return Redirect(Url.Home());
        }
    }
}
