using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using BaseApp.Common.Emails.Models;
using BaseApp.Common.Utils.Email;
using Microsoft.Extensions.Options;

namespace BaseApp.Common.Emails.Impl
{
    public class EmailSenderService: IEmailSenderService
    {
        private EmailSenderOptions Options { get; set; }

        public EmailSenderService(IOptions<EmailSenderOptions> options)
        {
            Options = options.Value;
        }

        public void SendEmail(
            IEnumerable<EmailAddressInfo> emailsTo,
            string subject,
            string bodyHtml,
            IEnumerable<EmailAddressInfo> emailsCc = null,
            IEnumerable<EmailAddressInfo> emailsBcc = null,
            Dictionary<string, byte[]> attachments = null)
        {
            if (emailsTo == null || !emailsTo.Any())
                throw new ArgumentNullException(nameof(emailsTo));

            ValidateWhiteList(emailsTo, emailsCc, emailsBcc);

            using (var letter = new MailMessage())
            {
                foreach (var emailAddressInfo in emailsTo)
                {
                    letter.To.Add(emailAddressInfo.ToMailAddress());
                }

                if (emailsCc != null)
                {
                    foreach (var emailAddressInfo in emailsCc)
                    {
                        letter.CC.Add(emailAddressInfo.ToMailAddress());
                    }
                }

                if (emailsBcc != null)
                {
                    foreach (var emailAddressInfo in emailsBcc)
                    {
                        letter.Bcc.Add(emailAddressInfo.ToMailAddress());
                    }
                }

                letter.Subject = subject;
                letter.IsBodyHtml = true;

                if (attachments != null)
                {
                    foreach (KeyValuePair<string, byte[]> attachment in attachments)
                        letter.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(attachment.Value), attachment.Key));
                }

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(bodyHtml, null, "text/html");
                letter.AlternateViews.Add(htmlView);
                
                using (var smtp = new SmtpClient(Options.Host, Options.Port))
                {
                    smtp.Credentials = new NetworkCredential(Options.UserName, Options.Password);
                    letter.From = new MailAddress(Options.FromEmail);
                    smtp.Send(letter);
                }
            }
        }

        private void ValidateWhiteList(IEnumerable<EmailAddressInfo> emailsTo,
            IEnumerable<EmailAddressInfo> emailsCc,
            IEnumerable<EmailAddressInfo> emailsBCc)
        {
            string[] notInWhiteListEmails = emailsTo
                .Union(emailsCc ?? new List<EmailAddressInfo>())
                .Union(emailsBCc ?? new List<EmailAddressInfo>())
                .Where(m => !Options.IsEmailAddressAllowed(m.Email))
                .Select(m => m.Email).ToArray();

            if (notInWhiteListEmails.Length > 0)
            {
                throw new Exception($"E-mail addresses is not allowed to send - \"{string.Join(", ", notInWhiteListEmails)}\"");
            }
        }
    }

    
}
