using BaseApp.Common;
using BaseApp.Data.Infrastructure;
using FluentValidation;

namespace BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;

public class RequestForgotPasswordArgsValidator : AbstractValidator<RequestForgotPasswordArgs>
{
    public RequestForgotPasswordArgsValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Email).MaximumLength(128).WithMessage("The Email field maximum length is 128 characters.")
            .Matches(Constants.RegularExpressions.Email).WithMessage(Constants.ValidationMessages.Email)
            .Custom((email, context) =>
            {
                var user = unitOfWork.Users.GetByEmailOrNull(email);
                if (user == null)
                {
                    context.AddFailure(nameof(RequestForgotPasswordArgs.Email), "The email address could not be found.");    
                }
            });
    }
}