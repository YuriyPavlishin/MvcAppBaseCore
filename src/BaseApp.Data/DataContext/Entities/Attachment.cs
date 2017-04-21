using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.DataContext.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        [Required, StringLength(256)]
        public string FileName { get; set; }
        [Required, StringLength(512)]
        public string GenFileName { get; set; }
        public long FileSize { get; set; }
        [Required, StringLength(256)]
        public string ContentType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }
    }
}
