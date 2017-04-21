using BaseApp.Common;
using BaseApp.Web.Code.Scheduler.Attributes;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels
{
    [SchedulerActionType(Enums.SchedulerActionTypes.ResetPasswordEmail)]
    public class ResetPasswordNotificationEmailModel : SchedulerModelBase
    {
        public int UserForgotPasswordId { get; set; }

        public ResetPasswordNotificationEmailModel(int createdByUserId) : base(createdByUserId)
        {
        }

        protected override void DoFillFromSchedulerData(SchedulerData schedulerData)
        {
            UserForgotPasswordId = schedulerData.EntityId1.Value;
        }

        protected override void DoFillSchedulerData(SchedulerData schedulerData)
        {
            schedulerData.EntityId1 = UserForgotPasswordId;
        }
    }
}