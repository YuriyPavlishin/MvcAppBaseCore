using BaseApp.Common;
using FluentValidation;

namespace BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;

public class CompleteForgotPasswordArgsValidator : AbstractValidator<CompleteForgotPasswordArgs>
{
    public CompleteForgotPasswordArgsValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(Constants.MinPasswordLength)
            .WithMessage($"The password must be at least {Constants.MinPasswordLength} characters long.");
        RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop).NotEmpty()
            .Equal(x => x.NewPassword).WithMessage("The new password and confirmation password do not match.");
    }
}