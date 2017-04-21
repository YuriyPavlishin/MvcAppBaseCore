using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Data.DataContext.Entities
{
    public class Role
    {
        public Role()
        {
            UserRoles = new List<UserRole>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required, StringLength(64)]
        public string Name { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}
