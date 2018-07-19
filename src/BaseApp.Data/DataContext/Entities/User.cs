using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseApp.Data.DataContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseApp.Data.DataContext.Entities
{
    public class User:IDeletable
    {
        public User()
        {
            UserRoles = new List<UserRole>();
            UpdatedUsers = new List<User>();
            DeletedUsers = new List<User>();
            UserForgotPasswords = new HashSet<UserForgotPassword>();
        }

        public int Id { get; set; }

        [Required, StringLength(64)]
        public string Login { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(64)]
        public string FirstName { get; set; }

        [Required, StringLength(64)]
        public string LastName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName { get; set;
            //get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
            //private set { }
        }

        [Required, StringLength(64)]
        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }

        //[ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(UpdatedUsers))]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey(nameof(UpdatedByUserId))]
        public virtual List<User> UpdatedUsers { get; set; }

        //[ForeignKey(nameof(DeletedByUserId))]
        [InverseProperty(nameof(DeletedUsers))]
        public virtual User DeletedByUser { get; set; }

        [ForeignKey(nameof(DeletedByUserId))]
        public virtual List<User> DeletedUsers { get; set; }

        public virtual ICollection<UserForgotPassword> UserForgotPasswords { get; set; }
    }

    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FullName).HasComputedColumnSql("ltrim(rtrim(FirstName + ' ' + LastName))");
        }
    }
}
