using System.ComponentModel.DataAnnotations;

namespace BaseApp.Web.Code.BLL.Admin.Users.Models;

public class UserListItemAdminModel
{
    public int Id { get; set; }
    [Display(Name = "Login")]
    public string Login { get; set; }
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Display(Name = "User Roles")]
    public string Roles { get; set; }
}