using System.Linq;
using AutoMapper;
using BaseApp.Common.Extensions;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataContext.Projections.Users;
using BaseApp.Web.Areas.Admin.Models.User;
using BaseApp.Web.Code.BLL.Admin.Users.Models;

namespace BaseApp.Web.Code.Mappers.Admin
{
    public class UserAdminMapper : MapperBase
    {
        public UserAdminMapper()
        {
            CreateMap<UserListItemAdminProjection, UserListItemAdminModel>()
                .Map(m => m.Roles, d => ", ".UseForJoinNonEmpty(d.Roles));
            CreateMap<User, UserForEditAdminModel>()
                .Map(x => x.Roles, x => x.UserRoles.Select(ur => ur.RoleId))
                .Ignore(x => x.DictionaryRoles);

            CreateMap<UserForEditAdminModel, UserEditModel>()
                .Ignore(x => x.Password)
                .Ignore(x => x.ConfirmPassword);


            CreateMap<UserEditModel, EditUserAdminArgs>();
            CreateMap<EditUserAdminArgs, User>(MemberList.Source)
                .IgnoreSource(x => x.Roles)
                .IgnoreSource(x => x.Password)
                .IgnoreSource(x => x.ConfirmPassword);
        }
    }
}
