using System.Collections.Generic;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;

namespace BaseApp.Data.DataRepository.Users
{
    public interface IRoleRepository : IRepositoryEntityBase<Role>
    {
        List<Role> GetAllRoles();
        Role GetUserRole();
        Role GetAdminRole();
    }
}
