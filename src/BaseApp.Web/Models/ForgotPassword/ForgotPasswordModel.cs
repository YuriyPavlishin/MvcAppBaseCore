using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Models.ForgotPassword
{
    public class ForgotPasswordModel : ValidatableModelBase
    {
        [EmailAddress, Required]
        public string Email { get; set; }

        protected override IEnumerable<ValidationResult> Validate(IUnitOfWork unitOfWork, Func<LoggedUserForValidationModel> getLoggedUser, ValidationContext validationContext)
        {
            var user = unitOfWork.Users.GetByEmailOrNull(Email);
            if (user == null)
            {
                yield return new ValidationResult("The email address could not be found.", new[] { nameof(Email) });
            }
        }
    }
}
