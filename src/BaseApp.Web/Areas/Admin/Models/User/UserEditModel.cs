using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Web.Areas.Admin.Models.User
{
    public class UserEditModel
    {
        public UserEditModel()
        {
            Roles = new List<int>();
        }

        public int? Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Roles")]
        [Required]
        public List<int> Roles { get; set; }
    }
}