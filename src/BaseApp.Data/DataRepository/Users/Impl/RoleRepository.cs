using System.Collections.Generic;
using System.Linq;
using BaseApp.Common;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository.Users.Impl
{
    public class RoleRepository(DataContextProvider context) : RepositoryEntityBase<Role>(context), IRoleRepository
    {
        public List<Role> GetAllRoles()
        {
            return Context.Set<Role>().ToList();
        }

        public Role GetUserRole()
        {
            return Context.Set<Role>().Single(m => m.Name == Constants.Roles.User);
        }

        public Role GetAdminRole()
        {
            return Context.Set<Role>().Single(m => m.Name == Constants.Roles.Admin);
        }
    }
}
