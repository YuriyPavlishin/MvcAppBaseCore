using System.Threading.Tasks;
using BaseApp.Web.Code.BLL.Admin.Users;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api.Admin;

public class UserController : ControllerBaseAdminApi
{
    [HttpGet]
    public async Task<ApiResult<UserListAdminModel>> GetList([FromServices] IUserQueryAdminManager queryManager, GetUsersAdminArgs args)
    {
        return await queryManager.GetListAsync(args);
    }
    
    [HttpGet]
    public ApiResult<UserForEditAdminModel> GetForEdit([FromServices] IUserQueryAdminManager queryManager, GetByIdOptionalArgs args)
    {
        return queryManager.GetForEdit(args);
    }
    
    [HttpPost]
    public async Task<ApiResult<EditUserResultAdminModel>> Edit([FromServices] IUserCommandAdminManager commandManager, EditUserAdminArgs args)
    {
        return await commandManager.EditAsync(args);
    }
    
    [HttpPost]
    public async Task<ApiResult> Delete([FromServices] IUserCommandAdminManager commandManager, GetByIdArgs args)
    {
        await commandManager.DeleteAsync(args);
        return ApiResult.Success();
    }
}