using BaseApp.Common;
using BaseApp.Data.Infrastructure;
using FluentValidation;

namespace BaseApp.Web.Code.BLL.Admin.Users.Models;

public class EditUserAdminArgsValidator : AbstractValidator<EditUserAdminArgs>
{
    public EditUserAdminArgsValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Login).NotEmpty()
            .Custom((login, context) =>
            {
                var userByLogin = unitOfWork.Users.GetByLoginOrNull(login, true);
                if (userByLogin != null && userByLogin.Id != context.InstanceToValidate.Id)
                {
                    context.AddFailure(nameof(EditUserAdminArgs.Login), "User name already in use.");    
                }
            });
        RuleFor(x => x.Email).NotEmpty()
            .Matches(Constants.RegularExpressions.Email).WithMessage(Constants.ValidationMessages.Email)
            .Custom((email, context) =>
            {
                var userByEmail = unitOfWork.Users.GetByEmailOrNull(email, true);
                if (userByEmail != null && userByEmail.Id != context.InstanceToValidate.Id)
                {
                    context.AddFailure(nameof(EditUserAdminArgs.Email), "Email already in use.");    
                }
            });
        RuleFor(x => x.Roles).NotEmpty().WithMessage("At least one role required.");
        RuleFor(x => x.Password).NotEmpty().When(x => x.Id == null);
        RuleFor(x => x.Password).Empty().When(x => x.Id != null);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).When(x => x.Id == null).WithMessage("The password and confirmation password do not match.");
    }
}