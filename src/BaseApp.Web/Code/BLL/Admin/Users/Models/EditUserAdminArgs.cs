using System.Collections.Generic;

namespace BaseApp.Web.Code.BLL.Admin.Users.Models;

public class EditUserAdminArgs
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public List<int> Roles { get; set; }
    
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class EditUserResultAdminModel
{
    public int Id { get; set; }
}