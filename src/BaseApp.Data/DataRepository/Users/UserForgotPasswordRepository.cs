using System;
using System.Linq;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DataRepository.Users;

public interface IUserForgotPasswordRepository : IRepositoryEntityBase<UserForgotPassword>
{
    UserForgotPassword GetRequest(Guid id);
}

public class UserForgotPasswordRepository(DataContextProvider context) : RepositoryEntityBase<UserForgotPassword>(context), IUserForgotPasswordRepository
{
    public UserForgotPassword GetRequest(Guid id)
    {
        return Context.Set<UserForgotPassword>()
            .Include(x => x.User)
            .SingleOrDefault(m => m.RequestGuid == id);
    }
}
