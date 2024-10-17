using System.Collections.Generic;

namespace BaseApp.Data.DataContext.Projections.Users;

public class UserListItemAdminProjection
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}