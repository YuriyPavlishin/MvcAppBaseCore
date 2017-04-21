using System.Collections.Generic;

namespace BaseApp.Web.Code.Scheduler.DataModels
{
    public class NotificationEmailData
    {
        public int Id { get; set; }
        public string BodyHtml { get; set; }
        public string Subject { get; set; }
        public IEnumerable<string> ToEmailAddresses { get; set; }
        public IEnumerable<string> ToCcEmailAddresses { get; set; }
        public IEnumerable<string> ToBccEmailAddresses { get; set; }
        public IEnumerable<NotificationAttachment> Attachments { get; set; }

        public NotificationEmailData()
        {
            ToEmailAddresses = new List<string>();
            ToCcEmailAddresses = new List<string>();
            ToBccEmailAddresses = new List<string>();
            Attachments = new List<NotificationAttachment>();
        }
    }
}