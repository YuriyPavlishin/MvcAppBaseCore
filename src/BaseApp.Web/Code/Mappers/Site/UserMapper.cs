using AutoMapper;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Models.Account;

namespace BaseApp.Web.Code.Mappers.Site
{
    public class UserMapper: MapperBase
    {
        public UserMapper()
        {
            CreateMap<User, UserProfileModel>();
            CreateMap<UserProfileModel, User>(MemberList.Source);
        }
    }
}
