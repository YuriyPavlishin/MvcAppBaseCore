using System;

namespace BaseApp.Web.Code.BLL.ForgotPassword.Models;

public class CompleteForgotPasswordArgs
{
    public Guid RequestId { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}