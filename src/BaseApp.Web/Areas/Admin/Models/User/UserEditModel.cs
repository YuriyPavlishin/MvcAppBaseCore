using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BaseApp.Common.Extensions;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Models;

namespace BaseApp.Web.Areas.Admin.Models.User
{
    public class UserEditModel : ValidatableModelBase
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

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Roles")]
        [Required]
        public List<int> Roles { get; set; }

        protected override IEnumerable<ValidationResult> Validate(IUnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext)
        {
            if (Roles == null || !Roles.Any())
            {
                yield return new ValidationResult("At least one role required.", new[] { this.GetPropertyName(m => m.Roles) });
            }

            var userByLogin = unitOfWork.Users.GetByLoginOrNull(Login, true);
            if ((userByLogin != null) && (userByLogin.Id != Id))
            {
                yield return new ValidationResult("User name already in use.", new[] { this.GetPropertyName(m => m.Login) });
            }

            var userByEmail = unitOfWork.Users.GetByEmailOrNull(Email, true);
            if ((userByEmail != null) && (userByEmail.Id != Id))
            {
                yield return new ValidationResult("Email already in use.", new[] { this.GetPropertyName(m => m.Email) });
            }
        }
    }
}