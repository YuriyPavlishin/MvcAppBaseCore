using System.Collections.Generic;
using BaseApp.Common.Utils;
using BaseApp.Data.Models;
using BaseApp.Web.Code.BLL.Admin.Users.Models;

namespace BaseApp.Web.Areas.Admin.Models.User
{
    public class UserListModel
    {
        public UserListArgs Args { get; set; }
        public List<UserListItemAdminModel> Items { get; set; }

        public UserListModel(UserListArgs args, List<UserListItemAdminModel> items)
        {
            Args = args;
            Items = items;
        }
    }

    public class UserListArgs
    {
        public string Search { get; set; }
        public PagingSortingInfo PagingSortingInfo { get; set; }

        public UserListArgs()
        {
            PagingSortingInfo = PagingSortingInfo.GetDefault(
                PropertyUtil.GetName<UserListItemAdminModel>(x => x.LastName)
            );
        }
    }
}
