using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BaseApp.Common.Emails.Models
{
    public class EmailAddressInfo
    {
        public string Email { get; private set; }
        public string DisplayName { get; private set; }

        public EmailAddressInfo(string email, string displayName = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }
            if (!ValidateEmail(email))
            {
                throw new Exception($"The specified string is not in the form required for an e-mail address - \"{email}\"");
            }
            Email = email;
            DisplayName = displayName;
        }

        public MailAddress ToMailAddress()
        {
            return new MailAddress(Email, DisplayName);
        }

        public static bool ValidateEmail(string p_Email)
        {
            if (String.IsNullOrEmpty(p_Email))
                return false;

            Regex r = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Match m = r.Match(p_Email.Trim());
            return m.Success;
        }

        public static bool ValidateEmailList(String p_Email)
        {
            if (string.IsNullOrEmpty(p_Email))
                return false;

            Regex r = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*(,\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*$");
            Match m = r.Match(p_Email.Trim());
            return m.Success;
        }
    }
}
