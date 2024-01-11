using System;
using System.Threading.Tasks;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using BaseApp.Web.Code.Scheduler;
using BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using BaseApp.Web.Models.ForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers
{
    public class ForgotPasswordController: ControllerBaseAuthorizeRequired
    {
        private readonly ISchedulerService _schedulerService;

        public ForgotPasswordController(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Index(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UnitOfWork.Users.GetByEmailOrNull(model.Email);

                var forgotPassword = new UserForgotPassword()
                {
                    CreatedDate = DateTime.Now,
                    CreatorIpAddress = new IpAddressResolver(HttpContext).GetUserHostIp(),
                    RequestGuid = Guid.NewGuid()
                };
                user.UserForgotPasswords.Add(forgotPassword);

                await UnitOfWork.SaveChangesAsync();

                var emailArgs = new ResetPasswordNotificationEmailModel(user.Id)
                {
                    UserForgotPasswordId = forgotPassword.Id
                };
                await _schedulerService.EmailSynchronizedAsync(emailArgs);

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
        public ActionResult CompleteResetPassword(Guid id)
        {
            var forgotPasswordRequest = UnitOfWork.Users.GetForgotPasswordRequest(id);
            string error;
            if (!TryValidateForgotPasswordRequest(forgotPasswordRequest, out error))
            {
                return View("CompleteResetPasswordError", error);
            }
            var model = new CompleteResetPasswordModel { RequestId = id };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CompleteResetPassword(CompleteResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var forgotPasswordRequest = UnitOfWork.Users.GetForgotPasswordRequest(model.RequestId);
            string error;
            if (!TryValidateForgotPasswordRequest(forgotPasswordRequest, out error))
            {
                return View("CompleteResetPasswordError", error);
            }

            forgotPasswordRequest.ApprovedDateTime = DateTime.Now;
            forgotPasswordRequest.ApproverIpAddress = new IpAddressResolver(HttpContext).GetUserHostIp();

            forgotPasswordRequest.User.Password = PasswordHash.HashPassword(model.NewPassword);
            forgotPasswordRequest.User.UpdatedDate = DateTime.Now;
            forgotPasswordRequest.User.UpdatedByUserId = forgotPasswordRequest.User.Id;

            UnitOfWork.SaveChanges();

            return RedirectToAction("CompleteResetPasswordSuccess");
        }

        [AllowAnonymous]
        public ActionResult CompleteResetPasswordSuccess()
        {
            return View();
        }

        private bool TryValidateForgotPasswordRequest(UserForgotPassword forgotPasswordRequest, out string error)
        {
            error = "";
            if (forgotPasswordRequest == null)
            {
                error = "Reset Password key not found";
            }
            else if (forgotPasswordRequest.ApprovedDateTime != null || forgotPasswordRequest.IsExpired)
            {
                error = "Reset Password url expired";
            }
            return string.IsNullOrEmpty(error);
        }
    }
}
