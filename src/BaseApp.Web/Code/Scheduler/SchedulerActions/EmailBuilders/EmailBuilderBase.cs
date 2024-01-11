using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Files;
using BaseApp.Web.Code.Scheduler.DataModels;
using BaseApp.Web.Code.Scheduler.SchedulerModels;

namespace BaseApp.Web.Code.Scheduler.SchedulerActions.EmailBuilders
{
    public abstract class EmailBuilderBase<T> : SchedulerActionBase<T>
        where T : SchedulerModelBase
    {
        protected EmailBuilderBase(SchedulerActionArgs args)
            : base(args)
        {
        }

        protected override async Task DoProcessAsync(T actionModel)
        {
            var emails = new List<NotificationEmailData>();
            await foreach (var email in BuildEmailsAsync(actionModel))
            {
                emails.Add(email);
            }

            using (var tran = UnitOfWork.BeginTransaction())
            {
                foreach (var email in emails)
                {
                    var attachments = new List<Attachment>();
                    foreach (var attachmentData in email.Attachments)
                    {
                        
                        var attachment = ActionArgs.Scope.GetService<IAttachmentService>().CreateAttachment(
                            UnitOfWork, actionModel.CreatedByUserId, attachmentData.FileName, attachmentData.GetFileBytes(), tran
                        );
                        attachments.Add(attachment);
                    }

                    var notificationEmail = UnitOfWork.Schedulers.CreateNotificationEmail();

                    notificationEmail.SchedulerId = actionModel.SchedulerId;
                    notificationEmail.CreatedDate = DateTime.Now;
                    notificationEmail.Body = email.BodyHtml;
                    notificationEmail.Subject = email.Subject;
                    notificationEmail.ToEmailAddresses = string.Join(",", email.ToEmailAddresses);
                    notificationEmail.ToCcEmailAddresses = string.Join(",", email.ToCcEmailAddresses);
                    notificationEmail.ToBccEmailAddresses = string.Join(",", email.ToBccEmailAddresses);
                    notificationEmail.NotificationEmailAttachments =
                        attachments.Select(m => new NotificationEmailAttachment() { Attachment = m }).ToList();
                }
                
                await UnitOfWork.SaveChangesAsync();
                tran.Commit();
            }
        }

        protected abstract IAsyncEnumerable<NotificationEmailData> BuildEmailsAsync(T model);
    }
}