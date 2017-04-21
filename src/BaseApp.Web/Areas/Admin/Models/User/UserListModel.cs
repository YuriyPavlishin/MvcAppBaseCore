using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Common.Utils;
using BaseApp.Data.Models;

namespace BaseApp.Web.Areas.Admin.Models.User
{
    public class UserListModel
    {
        public UserListArgs Args { get; set; }
        public List<UserListItemModel> Items { get; set; }

        public UserListModel(UserListArgs args, List<UserListItemModel> items)
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
                PropertyUtil.GetName<UserListItemModel>(x => x.LastName)
            );
        }
    }

    public class UserListItemModel
    {
        public int Id { get; set; }
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "User Roles")]
        public string Roles { get; set; }
    }
}
