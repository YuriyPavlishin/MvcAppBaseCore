using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApp.Common.Emails.Models;
using BaseApp.Common.Injection.Config;

namespace BaseApp.Common.Emails
{
    [Injectable(InjectableTypes.SingleInstance)]
    public interface IEmailSenderService
    {
        void SendEmail(
            IEnumerable<EmailAddressInfo> emailsTo,
            string subject,
            string bodyHtml,
            SendEmailArgs args = null);
    }
}
