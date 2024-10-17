using BaseApp.Common.Injection.Config;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;

namespace BaseApp.Web.Code.BLL.Site.ForgotPasswords;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IForgotPasswordQueryManager
{
    ForgotPasswordRequestModel GetRequest(GetRequestForgotPasswordArgs args);
    string GetRequestErrorMessage(UserForgotPassword args);
}