using BaseApp.Common.Injection.Config;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;

namespace BaseApp.Web.Code.BLL.ForgotPassword;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IForgotPasswordQueryManager
{
    ForgotPasswordRequestModel GetRequest(GetRequestForgotPasswordArgs args);
    string GetRequestErrorMessage(UserForgotPassword args);
}