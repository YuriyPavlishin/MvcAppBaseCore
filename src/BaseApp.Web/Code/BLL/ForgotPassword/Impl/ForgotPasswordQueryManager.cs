using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;

namespace BaseApp.Web.Code.BLL.ForgotPassword.Impl;

public class ForgotPasswordQueryManager(IUnitOfWork unitOfWork) : IForgotPasswordQueryManager
{
    public ForgotPasswordRequestModel GetRequest(GetRequestForgotPasswordArgs args)
    {
        var dbItem = unitOfWork.Users.ForgotPasswords.GetRequest(args.RequestID);
        var result = new ForgotPasswordRequestModel
        {
            ErrorMessage = GetRequestErrorMessage(dbItem)
        };
        return result;
    }

    public string GetRequestErrorMessage(UserForgotPassword args)
    {
        return args == null 
            ? ForgotPasswordRequestModel.NotFoundMessage 
            : (args.ApprovedDateTime != null || args.IsExpired)
                ? ForgotPasswordRequestModel.ExpiredMessage
                : "";
    }
}