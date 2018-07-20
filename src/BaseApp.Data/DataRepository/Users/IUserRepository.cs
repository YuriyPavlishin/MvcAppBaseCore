using System;
using System.Collections.Generic;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataContext.Projections.Users;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;

namespace BaseApp.Data.DataRepository.Users
{
    public interface IUserRepository : IRepositoryEntityDeletableBase<User>
    {
        IRoleRepository Roles { get; }
        List<User> GetUsersForAdmin(string search, PagingSortingInfo pagingSorting);
        List<User> GetUsersByFilter(string prefix, int count);
        User GetWithRolesOrNull(int id);
        User GetByEmailOrNull(string email, bool includeDeleted = false);
        User GetByLoginOrNull(string login, bool includeDeleted = false);
        AccountProjection GetAccountByLoginOrNull(string login);
        AccountProjection GetAccountByIdOrNull(int id);
        List<User> GetDeleted();
        UserForgotPassword GetForgotPasswordRequest(Guid id);
        UserForgotPassword GetForgotPasswordRequest(int id);
    }
}
