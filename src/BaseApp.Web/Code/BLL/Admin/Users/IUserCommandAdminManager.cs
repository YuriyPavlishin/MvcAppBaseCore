using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.Infrastructure.Validation;

namespace BaseApp.Web.Code.BLL.Admin.Users;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IUserCommandAdminManager
{
    Task<ValidatedValue<EditUserResultAdminModel>> EditAsync(EditUserAdminArgs args);
    Task DeleteAsync(GetByIdArgs args);
}