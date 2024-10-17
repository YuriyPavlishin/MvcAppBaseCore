namespace BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;

public class ForgotPasswordRequestModel
{
    public const string NotFoundMessage = "Reset Password key not found.";
    public const string ExpiredMessage = "Reset Password url expired.";
    public string ErrorMessage { get; set; }
}