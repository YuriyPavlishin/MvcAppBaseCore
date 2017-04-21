using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Data.DataContext.Entities
{
    public class UserForgotPassword
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid RequestGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(64)]
        public string CreatorIpAddress { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        [StringLength(64)]
        public string ApproverIpAddress { get; set; }

        [NotMapped]
        public bool IsExpired => CreatedDate.AddDays(1) < DateTime.Now;

        public virtual User User { get; set; }
    }
}
