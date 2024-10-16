﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Common;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataContext.Projections.Users;
using BaseApp.Data.Extensions;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DataRepository.Users.Impl
{
    internal static class UserExtensions
    {
        public static IQueryable<User> IncludeRoles(this IQueryable<User> users)
        {
            return users.Include(x => x.UserRoles).ThenInclude(x => x.Role);
        }

        public static IQueryable<AccountProjection> ToAccount(this IQueryable<User> users)
        {
            return users.Select(m => new AccountProjection
            {
                Id = m.Id,
                Login = m.Login,
                Password = m.Password,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                Roles = m.UserRoles.Select(t => t.Role.Name)
            });
        }
    }

    public class UserRepository(DataContextProvider context) : RepositoryEntityDeletableBase<User>(context), IUserRepository
    {
        public IRoleRepository Roles => GetRepository<RoleRepository>();
        public IUserForgotPasswordRepository ForgotPasswords => GetRepository<UserForgotPasswordRepository>();

        public async Task<List<UserListItemAdminProjection>> GetUsersForAdminAsync(string search, PagingSortingInfo pagingSorting)
        {
            var query = EntitySetNotDeleted.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => x.FullName.Contains(search));
            }
            return await query.Select(x => new UserListItemAdminProjection
            {
                Id = x.Id, Login = x.Login, FirstName = x.FirstName, LastName = x.LastName, Email = x.Email, Roles = x.UserRoles.Select(t => t.Role.Name).ToList()
            }).PagingSorting(pagingSorting).ToListAsync();
        }

        public List<User> GetUsersByFilter(string prefix, int count)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = null;
            return EntitySetNotDeleted
                .Where(m => (prefix == null || m.FirstName.StartsWith(prefix) || m.LastName.StartsWith(prefix)))
                .Take(count)
                .ToList();
        }

        public User GetWithRolesOrNull(int id)
        {
            return EntitySetNotDeleted.IncludeRoles()
                .FirstOrDefault(m => m.Id == id);
        }

        public User GetByEmailOrNull(string email, bool includeDeleted = false)
        {
            return GetUserView(includeDeleted).IncludeRoles()
                .FirstOrDefault(m => m.Email == email);
        }

        public User GetByLoginOrNull(string login, bool includeDeleted = false)
        {
            return GetUserView(includeDeleted).IncludeRoles()
                .FirstOrDefault(m => m.Login == login);
        }

        public AccountProjection GetAccountByLoginOrNull(string login)
        {
            return EntitySetNotDeleted.Where(m => m.Login == login)
                .ToAccount().FirstOrDefault();
        }

        public AccountProjection GetAccountByIdOrNull(int id)
        {
            return EntitySet.Where(m => m.Id == id)
                .ToAccount().FirstOrDefault();
        }
        
        public AccountProjection GetFirstAdminAccount()
        {
            return Context.Set<UserRole>().Where(x => x.Role.Name == Constants.Roles.Admin)
                .Select(x => x.User).GetNotDeleted().ToAccount().First();
        }

        private IQueryable<User> GetUserView(bool includeDeleted = false)
        {
            return includeDeleted ? EntitySet : EntitySetNotDeleted;
        }

        public List<User> GetDeleted()
        {
            return EntitySet.Where(x => x.DeletedDate != null).ToList();
        }

        public override User CreateEmpty()
        {
            var item = base.CreateEmpty();
            item.CreatedDate = DateTime.Now;
            return item;
        }
    }
}
