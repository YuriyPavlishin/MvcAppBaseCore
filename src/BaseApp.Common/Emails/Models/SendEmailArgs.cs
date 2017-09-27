using System.Collections.Generic;

namespace BaseApp.Common.Emails.Models
{
    public class SendEmailArgs
    {
        public IEnumerable<EmailAddressInfo> EmailsCc { get; set; }
        public IEnumerable<EmailAddressInfo> EmailsBcc { get; set; }
        public Dictionary<string, byte[]> Attachments { get; set; }
        public IEnumerable<EmailAddressInfo> EmailsReplyTo { get; set; }        
        public Dictionary<string, string> EmailHeaders { get; set; }

        public string FromDisplayNameOverride { get; set; }
        public string FromEmailOverride { get; set; }
    }
}
