using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task LoadAndProcessAsync()
        {
            await DoLoadAndProcessAsync();
        }

        public Task ProcessSchedulerSynchronizedAsync(int schedulerId)
        {
            return DoLoadAndProcessAsync(schedulerId, true);
        }

        private async Task DoLoadAndProcessAsync(int? schedulerId = null, bool isSync = false)
        {
            NotificationEmail notificationEmail;
            do
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    using (var tran = unitOfWork.BeginTransaction())
                    {
                        notificationEmail = await unitOfWork.Schedulers.GetNextEmailToProcessAsync(schedulerId, isSync);
                        if (notificationEmail == null)
                            continue;
                        notificationEmail.ProcessedDate = DateTime.Now;
                        await unitOfWork.SaveChangesAsync();
                        tran.Commit();
                    }
                    
                    await ProcessItemAsync(MapNotificationEmail(notificationEmail), isSync);
                }
            } while (notificationEmail != null);
        }

        private async Task ProcessItemAsync(NotificationEmailData emailData, bool isSync)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    var notificationEmail = unitOfWork.Schedulers.GetNotificationEmail(emailData.Id);
                    notificationEmail.LastAttemptDate = DateTime.Now;
                    notificationEmail.LastAttemptError = null;
                    notificationEmail.AttemptsCount++;

                    using (var tran = unitOfWork.BeginTransaction())
                    {
                        await unitOfWork.SaveChangesAsync();

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
                await MarkFailedAsync(emailData, ex, isSync);

                if (isSync)
                    throw;
            }
        }

        private async Task MarkFailedAsync(NotificationEmailData emailData, Exception ex, bool isSync)
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
                    else
                    {
                        notificationEmail.ProcessedDate = null;
                    }

                    await unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                LogHolder.MainLog.Error(e, "Error occured while saving email data in failed state - " + emailData.Id);
            }
        }

        private NotificationEmailData MapNotificationEmail(NotificationEmail m)
        {
            return new NotificationEmailData
            {
                Id = m.Id,
                Subject = m.Subject,
                BodyHtml = m.Body,
                ToEmailAddresses = MapEmailAddresses(m.ToEmailAddresses),
                ToCcEmailAddresses = MapEmailAddresses(m.ToCcEmailAddresses),
                ToBccEmailAddresses = MapEmailAddresses(m.ToBccEmailAddresses),
                Attachments = m.NotificationEmailAttachments.Select(t 
                    => NotificationAttachment.Create(t.Attachment.FileName, fileFactoryService.Attachments.GetFilePath(t.Attachment.GenFileName)))
            };
        }

        private static IEnumerable<string> MapEmailAddresses(string emails)
        {
            return emails
                .Split(',')
                .Where(t => !string.IsNullOrWhiteSpace(t));
        }

        private static IEnumerable<EmailAddressInfo> MapEmailAddresses(IEnumerable<string> emails)
        {
            return emails.Select(m => new EmailAddressInfo(m));
        }
    }
}