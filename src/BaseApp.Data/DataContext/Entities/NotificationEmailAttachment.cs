using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.DataContext.Entities
{
    public class NotificationEmailAttachment
    {
        public int Id { get; set; }
        public int NotificationEmailId { get; set; }
        public int AttachmentId { get; set; }

        public virtual NotificationEmail NotificationEmail { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
