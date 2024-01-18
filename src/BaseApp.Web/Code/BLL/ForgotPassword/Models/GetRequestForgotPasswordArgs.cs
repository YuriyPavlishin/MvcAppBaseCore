using System;
using BaseApp.Web.Code.Infrastructure;

namespace BaseApp.Web.Code.BLL.ForgotPassword.Models;

public class GetRequestForgotPasswordArgs
{
    [NotDefaultValueRequired]
    public Guid RequestID { get; set; }
}