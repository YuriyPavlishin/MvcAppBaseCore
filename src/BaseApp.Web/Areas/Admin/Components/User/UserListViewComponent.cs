using System.Collections.Generic;
using BaseApp.Web.Areas.Admin.Models.User;
using BaseApp.Web.Code.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Areas.Admin.Components.User
{
    public class UserListViewComponent: ViewComponentBase
    {
        public IViewComponentResult Invoke(UserListArgs args)
        {
            var items = Mapper.Map<List<UserListItemModel>>(
                UnitOfWork.Users.GetUsersForAdmin(args.Search, args.PagingSortingInfo)
            );

            return View(new UserListModel(args, items));
        }
    }
}
