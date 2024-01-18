using System;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository.Users;

public interface IUserForgotPasswordRepository : IRepositoryEntityBase<UserForgotPassword>
{
    UserForgotPassword GetRequest(Guid id);
}