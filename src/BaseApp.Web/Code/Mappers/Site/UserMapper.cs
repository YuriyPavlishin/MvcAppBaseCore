using AutoMapper;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;
using BaseApp.Web.Models.Account;
using BaseApp.Web.Models.ForgotPassword;

namespace BaseApp.Web.Code.Mappers.Site
{
    public class UserMapper: MapperBase
    {
        public UserMapper()
        {
            CreateMap<User, UserProfileModel>();
            CreateMap<UserProfileModel, User>(MemberList.Source);
            CreateMap<CompleteResetPasswordModel, CompleteForgotPasswordArgs>();
        }
    }
}
