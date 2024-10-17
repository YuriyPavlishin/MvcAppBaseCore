using System.Collections.Generic;
using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.BLL.Admin.Users.Models;

public class UserForEditAdminModel
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public List<int> Roles { get; set; }
    
    public List<DataItemModel> DictionaryRoles { get; set; }
}