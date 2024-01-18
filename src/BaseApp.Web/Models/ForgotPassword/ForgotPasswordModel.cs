using System.ComponentModel.DataAnnotations;

namespace BaseApp.Web.Models.ForgotPassword;

public class ForgotPasswordModel
{
    [EmailAddress, Required]
    public string Email { get; set; }
}