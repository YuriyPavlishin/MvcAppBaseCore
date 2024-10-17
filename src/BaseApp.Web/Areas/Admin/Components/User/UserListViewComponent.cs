using System.Threading.Tasks;
using BaseApp.Web.Areas.Admin.Models.User;
using BaseApp.Web.Code.BLL.Admin.Users;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Areas.Admin.Components.User
{
    public class UserListViewComponent(IUserQueryAdminManager queryManager): ViewComponentBase
    {
        public async Task<IViewComponentResult> InvokeAsync(UserListArgs args)
        {
            var result = await queryManager.GetListAsync(new GetUsersAdminArgs
            {
                Query = args.Search,
                PagingSorting = Mapper.Map<PagingSortingArgs>(args.PagingSortingInfo)
            });
            args.PagingSortingInfo = result.PagingSortingInfo;
            return View(new UserListModel(args, result.Items));
        }
    }
}
