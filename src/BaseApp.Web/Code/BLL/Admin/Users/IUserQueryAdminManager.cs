using System.Threading.Tasks;
using BaseApp.Common.Injection.Config;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.BLL.Admin.Users;

[Injectable(InjectableTypes.LifetimeScope)]
public interface IUserQueryAdminManager
{
    Task<UserListAdminModel> GetListAsync(GetUsersAdminArgs args);
    UserForEditAdminModel GetForEdit(GetByIdOptionalArgs args);
}