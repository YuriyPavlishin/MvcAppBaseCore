using System.Linq;
using AutoMapper;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Areas.Admin.Models.User;
using BaseApp.Web.Code.Extensions;

namespace BaseApp.Web.Code.Mappers.Admin
{
    public class UserMapperAdmin : MapperBase
    {
        public UserMapperAdmin()
        {
            CreateMap<User, UserListItemModel>()
                .Map(m => m.Roles, d => string.Join(", ", d.UserRoles.Select(c => c.Role.Name)));

            CreateMap<User, UserEditModel>()
                .Map(m => m.LastName, t => t.LastName)
                .Map(x => x.Roles, x => x.UserRoles.Select(ur => ur.RoleId))
                .Map(x => x.ConfirmPassword, x => x.Password);


            CreateMap<UserEditModel, User>(MemberList.Source)
                .IgnoreSource(x => x.Roles)
                .IgnoreSource(x => x.ConfirmPassword);
        }
    }
}
