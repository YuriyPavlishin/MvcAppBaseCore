using System.Collections.Generic;
using BaseApp.Common.Emails.Models;

namespace BaseApp.Common.Emails
{
    public interface IEmailSenderService
    {
        void SendEmail(
            IEnumerable<EmailAddressInfo> emailsTo,
            string subject,
            string bodyHtml,
            IEnumerable<EmailAddressInfo> emailsCc = null,
            IEnumerable<EmailAddressInfo> emailsBcc = null,
            Dictionary<string, byte[]> attachments = null);
    }
}
