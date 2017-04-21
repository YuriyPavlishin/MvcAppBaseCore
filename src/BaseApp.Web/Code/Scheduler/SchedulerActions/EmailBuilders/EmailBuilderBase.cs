using System;
using System.Collections.Generic;
using System.Linq;
using BaseApp.Data.DataContext.Entities;
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

        protected override void DoProcess(T actionModel)
        {
            var emails = BuildEmails(actionModel);

            using (var tran = UnitOfWork.BeginTransaction())
            {
                foreach (var email in emails)
                {
                    var attachments = new List<Attachment>();
                    foreach (var attachmentData in email.Attachments)
                    {
                        
                        var attachment = ActionArgs.AttachmentService.CreateAttachment(
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
                
                UnitOfWork.SaveChanges();
                tran.Commit();
            }
        }

        public abstract IEnumerable<NotificationEmailData> BuildEmails(T model);
    }
}