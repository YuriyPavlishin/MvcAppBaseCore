using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using BaseApp.Common.Emails.Models;
using BaseApp.Common.Utils.Email;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.Threading.Tasks;
using BaseApp.Common.Utils;

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
            SendEmailArgs args = null)
        {
            if (emailsTo == null || !emailsTo.Any())
                throw new ArgumentNullException(nameof(emailsTo));

            ValidateWhiteList(emailsTo, args.EmailsCc, args.EmailsBcc);

            using (var letter = new MailMessage())
            {
                letter.Subject = subject;
                letter.IsBodyHtml = true;

                var htmlView = AlternateView.CreateAlternateViewFromString(bodyHtml, null, "text/html");
                letter.AlternateViews.Add(htmlView);

                AddEmailsAddresses(letter.To, emailsTo);
                AddEmailsAddresses(letter.CC, args.EmailsCc);
                AddEmailsAddresses(letter.Bcc, args.EmailsBcc);
                AddEmailsAddresses(letter.ReplyToList, args.EmailsReplyTo);
                AddHeaders(letter.Headers, args.EmailHeaders);
                AddAttachments(letter.Attachments, args.Attachments);
                AddLinkedResources(htmlView.LinkedResources, args.LinkedResources);

                using (var smtp = new SmtpClient(Options.Host, Options.Port))
                {
                    smtp.Credentials = new NetworkCredential(Options.UserName, Options.Password);
                    letter.From = GetFromMailAddress(args);
                    smtp.Send(letter);
                }
            }
        }       

        private void AddEmailsAddresses(MailAddressCollection addressCollection, IEnumerable<EmailAddressInfo> emails)
        {
            foreach (var emailAddressInfo in emails ?? Enumerable.Empty<EmailAddressInfo>())
                addressCollection.Add(emailAddressInfo.ToMailAddress());
        }

        private void AddAttachments(AttachmentCollection attachmentCollection, Dictionary<string, byte[]> attachments)
        {
            foreach (var attachment in attachments ?? Enumerable.Empty<KeyValuePair<string, byte[]>>())
                attachmentCollection.Add(new Attachment(new MemoryStream(attachment.Value), attachment.Key));
        }

        private void AddHeaders(NameValueCollection headersCollection, Dictionary<string, string> emailHeaders)
        {
            foreach (var header in emailHeaders ?? Enumerable.Empty<KeyValuePair<string, string>>())
                headersCollection.Add(header.Key, header.Value);
        }

        private void AddLinkedResources(LinkedResourceCollection linkedResourceCollection, Dictionary<string, byte[]> linkedResources)
        {
            foreach (var linkedResource in linkedResources ?? Enumerable.Empty<KeyValuePair<string, byte[]>>())
            {
                var lres = new LinkedResource(new MemoryStream(linkedResource.Value), MimeTypeResolver.Resolve(linkedResource.Key))
                {
                    ContentId = linkedResource.Key
                };
                linkedResourceCollection.Add(lres);
            }
        }

        private MailAddress GetFromMailAddress(SendEmailArgs args)
        {
            string emailFrom = string.IsNullOrWhiteSpace(args?.FromEmailOverride)
                ? Options.FromEmail
                : args.FromEmailOverride;

            string emailFromDisplayName = string.IsNullOrWhiteSpace(args?.FromDisplayNameOverride)
                ? Options.FromDisplayName
                : args.FromDisplayNameOverride;

            return new MailAddress(emailFrom, emailFromDisplayName);
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
