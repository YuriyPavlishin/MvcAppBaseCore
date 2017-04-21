using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Models
{
    public abstract class ValidatableModelBase: IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Validate(AppDependencyResolver.Current.GetScopedUoW(), AppDependencyResolver.Current.GetLoggedUser(), validationContext);
        }

        protected abstract IEnumerable<ValidationResult> Validate(UnitOfWork unitOfWork, ILoggedUserAccessor loggedUser, ValidationContext validationContext);
    }
}
