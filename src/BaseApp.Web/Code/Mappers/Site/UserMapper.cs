using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Code.Extensions;
using BaseApp.Web.Models.Account;

namespace BaseApp.Web.Code.Mappers.Site
{
    public class UserMapper: MapperBase
    {
        protected override void CreateMaps()
        {
            //CreateMap<Role, Areas.Admin.Models.UserRoleModel>()
            //    .Ignore(m => m.Checked);

            //CreateMap<Areas.Admin.Models.UserRoleModel, Role>()
            //    .Ignore(m => m.UserRoles);
            CreateMap<User, UserProfileModel>();

            CreateMap<UserProfileModel, User>()
               .IgnoreAll() //Prefer .IgnoreAllUnmappedComplexTypes() method when it's possible
               .Map(m => m.Login, t => t.Login)
               .Map(m => m.FirstName, t => t.FirstName)
               .Map(m => m.LastName, t => t.LastName)
               .Map(m => m.Email, t => t.Email);
        }
    }
}
