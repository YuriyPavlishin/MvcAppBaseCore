using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Data.Infrastructure;
using BaseApp.Common.Extensions;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Models.Account
{
    public class UserProfileModel : ValidatableModelBase
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        protected override IEnumerable<ValidationResult> Validate(IUnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext)
        {
            if (!string.Equals(loggedUser.Claims.Login, Login, StringComparison.OrdinalIgnoreCase))
            {
                var byLogin = unitOfWork.Users.GetByLoginOrNull(Login, true);
                if (byLogin != null && byLogin.Id != loggedUser.Id)
                {
                    yield return new ValidationResult("Login already in use.", new[] { this.GetPropertyName(m => m.Login) });
                }
            }
        }
    }
}