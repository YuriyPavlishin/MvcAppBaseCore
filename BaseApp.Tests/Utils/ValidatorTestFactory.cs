using System.Threading;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace BaseApp.Tests.Utils;

public static class ValidatorTestFactory
{
    public static Mock<IValidator<TH>> CreateSuccessMock<TH>() where TH: class
    {
        var holder = new Mock<IValidator<TH>>();
        holder.Setup(x => x.ValidateAsync(It.IsAny<TH>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => new ValidationResult());
        return holder;
    }
}