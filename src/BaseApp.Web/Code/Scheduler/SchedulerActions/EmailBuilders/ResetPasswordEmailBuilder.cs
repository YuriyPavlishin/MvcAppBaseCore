using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.CustomRazor;
using BaseApp.Web.Code.Scheduler.DataModels;
using BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using BaseApp.Web.Models.TemplateModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions.EmailBuilders
{
    public class ResetPasswordEmailBuilder(SchedulerActionArgs args) : EmailBuilderBase<ResetPasswordNotificationEmailModel>(args)
    {
        protected override async IAsyncEnumerable<NotificationEmailData> BuildEmailsAsync(ResetPasswordNotificationEmailModel model)
        {
            var forgot = UnitOfWork.Users.ForgotPasswords.Get(model.UserForgotPasswordId);

            var user = UnitOfWork.Users.Get(forgot.UserId);

            var emailModel = new ResetPasswordModel
            {
                RequestIp = forgot.CreatorIpAddress,
                ResetPasswordUrl = ActionArgs.Scope.GetService<IPathResolver>().BuildFullUrl("/ForgotPassword/CompleteResetPassword?id=" + forgot.RequestGuid),
                UserName = user.FullName
            };

            var res = new NotificationEmailData();
            res.BodyHtml = await ActionArgs.Scope.GetService<ICustomRazorViewService>().RenderAsync("ResetPassword", emailModel);
            res.Subject = "Password reset confirmation";
            res.ToEmailAddresses = new[] { user.Email };

            yield return res;
        }
    }
}