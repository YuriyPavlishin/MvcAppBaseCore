using System.Linq;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Areas.Admin.Models.User;
using BaseApp.Web.Code.Extensions;

namespace BaseApp.Web.Code.Mappers.Admin
{
    public class UserMapperAdmin : MapperBase
    {
        protected override void CreateMaps()
        {
            CreateMap<User, UserListItemModel>()
                .Map(m => m.Roles, d => string.Join(", ", d.UserRoles.Select(c => c.Role.Name)));


            CreateMap<User, UserEditModel>()
                .Map(x => x.Roles, x => x.UserRoles.Select(ur => ur.RoleId))
                .Map(x => x.ConfirmPassword, x => x.Password);

            CreateMap<UserEditModel, User>()
                 .IgnoreAll() //Prefer .IgnoreAllUnmappedComplexTypes() method when it's possible
                 .Map(m => m.FirstName, t => t.FirstName)
                 .Map(m => m.LastName, t => t.LastName)
                 .Map(m => m.Email, t => t.Email)
                 .Map(m => m.Login, t => t.Login)
                 .Map(m => m.Password, t => t.Password);
        }
    }
}
