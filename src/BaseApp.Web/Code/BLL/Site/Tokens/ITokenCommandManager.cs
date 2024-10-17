using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.BLL.Site.Tokens.Models;

namespace BaseApp.Web.Code.BLL.Site.Tokens;

[Injectable(InjectableTypes.LifetimeScope)]
public interface ITokenCommandManager
{
    TokenModel Retrieve(RetrieveTokenArgs args);
}