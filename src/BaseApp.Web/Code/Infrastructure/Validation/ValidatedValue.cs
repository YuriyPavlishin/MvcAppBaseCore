using System.Linq;
using BaseApp.Common.Extensions;
using BaseApp.Web.Code.Infrastructure.Api;

namespace BaseApp.Web.Code.Infrastructure.Validation;

public class ValidatedValue<T>: ValidatedValue
{
    private readonly T _result;

    public ValidatedValue(ValidationItemModel[] validationItems, T result) :base(validationItems)
    {
        _result = result;
    }

    public T EnsuredValue
    {
        get
        {
            Ensure();
            return _result;
        }
    }
}

public class ValidatedValue
{
    public ValidationItemModel[] ValidationItems { get; }
    public bool IsValid => ValidationItems.IsNullOrEmpty();
    public ValidatedValue(ValidationItemModel[] validationItems)
    {
        ValidationItems = validationItems;
    }

    public void Ensure()
    {
        if (ValidationItems.IsNullOrEmpty())
            return;
        var message = ApiException.GetArgumentValidationFailedMessage(ValidationItems.Select(x => $"{x.PropertyName}: {x.ErrorMessage}").ToList());
        throw new ApiException(ApiResult.ApiResultCodes.ArgumentValidationFailed,  message);
    }
}

public class ValidationItemModel
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}