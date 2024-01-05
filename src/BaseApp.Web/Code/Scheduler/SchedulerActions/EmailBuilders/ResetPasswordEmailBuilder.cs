using System.Collections.Generic;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.CustomRazor;
using BaseApp.Web.Code.Scheduler.DataModels;
using BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using BaseApp.Web.Models.TemplateModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions.EmailBuilders
{
    public class ResetPasswordEmailBuilder : EmailBuilderBase<ResetPasswordNotificationEmailModel>
    {
        public ResetPasswordEmailBuilder(SchedulerActionArgs args) : base(args)
        {
        }

        public override IEnumerable<NotificationEmailData> BuildEmails(ResetPasswordNotificationEmailModel model)
        {
            var forgot = UnitOfWork.Users.GetForgotPasswordRequest(model.UserForgotPasswordId);

            var user = UnitOfWork.Users.Get(forgot.UserId);

            var emailModel = new ResetPasswordModel
            {
                RequestIp = forgot.CreatorIpAddress,
                ResetPasswordUrl = ActionArgs.Scope.GetService<IPathResolver>().BuildFullUrl("/ForgotPassword/CompleteResetPassword?id=" + forgot.RequestGuid),
                //ResetPasswordUrl = "TEST",
                UserName = user.FullName
            };

            var res = new NotificationEmailData();
            res.BodyHtml = ActionArgs.Scope.GetService<ICustomRazorViewService>().Render("ResetPassword", emailModel);
            res.Subject = "Password reset confirmation";
            res.ToEmailAddresses = new[] { user.Email };

            yield return res;
        }
    }
}