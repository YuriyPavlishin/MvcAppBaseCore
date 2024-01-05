using System;
using System.Collections.Generic;
using System.Linq;
using BaseApp.Common.Emails;
using BaseApp.Common.Emails.Models;
using BaseApp.Common.Files;
using BaseApp.Common.Logs;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Scheduler.DataModels;

namespace BaseApp.Web.Code.Scheduler.Queue.Workers.Impl
{
    public class EmailWorkerService(
        IEmailSenderService emailSenderService,
        IFileFactoryService fileFactoryService,
        Func<IUnitOfWorkPerCall> unitOfWorkPerCallFunc)
        : WorkerServiceBase(unitOfWorkPerCallFunc), IEmailWorkerService
    {
        private const int MAX_ATTEMPTS_COUNT = 5;

        public override void LoadAndProcess()
        {
            DoLoadAndProcess();
        }

        public void ProcessSchedulerSync(int schedulerId)
        {
            DoLoadAndProcess(schedulerId, true);
        }

        private void DoLoadAndProcess(int? schedulerId = null, bool isSync = false)
        {
            List<NotificationEmailData> emails;
            using (var unitOfWork = CreateUnitOfWork())
            {
                var emailFromDb = unitOfWork.Schedulers.GetEmailsToProcess(schedulerId, isSync);
                emails = MapNotificationEmails(emailFromDb);
            }

            foreach (var emailData in emails)
            {
                try
                {
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        var notificationEmail = unitOfWork.Schedulers.GetNotificationEmail(emailData.Id);
                        notificationEmail.ProcessedDate = DateTime.Now;
                        notificationEmail.LastAttemptDate = DateTime.Now;
                        notificationEmail.LastAttemptError = null;
                        notificationEmail.AttemptsCount++;

                        using (var tran = unitOfWork.BeginTransaction())
                        {
                            unitOfWork.SaveChanges();

                            emailSenderService.SendEmail(
                                MapEmailAddresses(emailData.ToEmailAddresses),
                                emailData.Subject,
                                emailData.BodyHtml,
                                new SendEmailArgs
                                {
                                    EmailsCc = MapEmailAddresses(emailData.ToCcEmailAddresses),
                                    EmailsBcc = MapEmailAddresses(emailData.ToBccEmailAddresses),
                                    Attachments = emailData.Attachments.ToDictionary(m => m.FileName, m => m.GetFileBytes())
                                });

                            tran.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHolder.MainLog.Error(ex, "Error send email - " + emailData.Id);

                    try
                    {
                        using (var unitOfWork = CreateUnitOfWork())
                        {
                            var notificationEmail = unitOfWork.Schedulers.GetNotificationEmail(emailData.Id);
                            notificationEmail.LastAttemptDate = DateTime.Now;
                            notificationEmail.LastAttemptError = ex.GetBaseException().Message;
                            notificationEmail.AttemptsCount++;

                            if (isSync || notificationEmail.AttemptsCount > MAX_ATTEMPTS_COUNT)
                            {
                                notificationEmail.ProcessedDate = DateTime.Now;
                            }

                            unitOfWork.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        LogHolder.MainLog.Error(e, "Error occured while saving email data in failed state - " + emailData.Id);
                    }

                    if (isSync)
                        throw;
                }
            }
        }

        private List<NotificationEmailData> MapNotificationEmails(IEnumerable<NotificationEmail> notificationEmails)
        {
            return notificationEmails.Select(m => new NotificationEmailData
            {
                Id = m.Id,
                Subject = m.Subject,
                BodyHtml = m.Body,
                ToEmailAddresses = MapEmailAddresses(m.ToEmailAddresses),
                ToCcEmailAddresses = MapEmailAddresses(m.ToCcEmailAddresses),
                ToBccEmailAddresses = MapEmailAddresses(m.ToBccEmailAddresses),
                Attachments = m.NotificationEmailAttachments.Select(t 
                    => NotificationAttachment.Create(t.Attachment.FileName, fileFactoryService.Attachments.GetFilePath(t.Attachment.GenFileName)))
            }).ToList();
        }

        private IEnumerable<String> MapEmailAddresses(string emails)
        {
            return emails
                .Split(',')
                .Where(t => !String.IsNullOrWhiteSpace(t));
        }

        private IEnumerable<EmailAddressInfo> MapEmailAddresses(IEnumerable<string> emails)
        {
            return emails.Select(m => new EmailAddressInfo(m));
        }
    }
}