using BaseApp.Common.Injection.Config;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;

namespace BaseApp.Web.Code.BLL.Site.ForgotPasswords;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IForgotPasswordQueryManager
{
    ForgotPasswordRequestModel GetRequest(GetRequestForgotPasswordArgs args);
    string GetRequestErrorMessage(UserForgotPassword args);
}

public class ForgotPasswordQueryManager(IUnitOfWork unitOfWork) : IForgotPasswordQueryManager
{
    public ForgotPasswordRequestModel GetRequest(GetRequestForgotPasswordArgs args)
    {
        var dbItem = unitOfWork.Users.ForgotPasswords.GetRequest(args.RequestID);
        var result = new ForgotPasswordRequestModel
        {
            ErrorMessage = GetRequestErrorMessage(dbItem)
        };
        return result;
    }

    public string GetRequestErrorMessage(UserForgotPassword args)
    {
        return args == null
            ? ForgotPasswordRequestModel.NotFoundMessage
            : (args.ApprovedDateTime != null || args.IsExpired)
                ? ForgotPasswordRequestModel.ExpiredMessage
                : "";
    }
}
