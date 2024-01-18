using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;
using BaseApp.Web.Code.Infrastructure.Validation;

namespace BaseApp.Web.Code.BLL.ForgotPassword;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IForgotPasswordCommandManager
{
    Task<ValidatedValue> RequestAsync(RequestForgotPasswordArgs args);
    Task<ValidatedValue<ForgotPasswordRequestModel>> CompleteAsync(CompleteForgotPasswordArgs args);
}