using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.Injection;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Web.Models
{
    public abstract class ValidatableModelBase: IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Validate(validationContext.GetService<IUnitOfWork>(), () => AppDependencyResolver.Current.GetLoggedUser(), validationContext);
        }

        protected abstract IEnumerable<ValidationResult> Validate(IUnitOfWork unitOfWork, Func<LoggedUserForValidationModel> GetLoggedUser, ValidationContext validationContext);
    }
}
