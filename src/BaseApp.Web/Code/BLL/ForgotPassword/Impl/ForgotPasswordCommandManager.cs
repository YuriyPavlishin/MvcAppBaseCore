using System;
using System.Threading.Tasks;
using BaseApp.Common.Utils;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;
using BaseApp.Web.Code.Infrastructure.ClientRequests;
using BaseApp.Web.Code.Infrastructure.Validation;
using BaseApp.Web.Code.Scheduler;
using BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using FluentValidation;

namespace BaseApp.Web.Code.BLL.ForgotPassword.Impl;

public class ForgotPasswordCommandManager(IUnitOfWork unitOfWork
    , IValidator<RequestForgotPasswordArgs> requestForgotPasswordValidator, IValidator<CompleteForgotPasswordArgs> completeForgotPasswordValidator
    , IForgotPasswordQueryManager forgotPasswordQueryManager, ISchedulerService schedulerService, IClientRequestAccessor clientRequestAccessor) : IForgotPasswordCommandManager
{
    public async Task<ValidatedValue> RequestAsync(RequestForgotPasswordArgs args)
    {
        return await ValidatedValueFactory.CreateAsync(
            await requestForgotPasswordValidator.ValidateAsync(args), async () => await RequestInnerAsync(args)
        );
    }
    
    private async Task RequestInnerAsync(RequestForgotPasswordArgs args)
    {
        var user = unitOfWork.Users.GetByEmailOrNull(args.Email);
        
        var dbItem = unitOfWork.Users.ForgotPasswords.CreateEmpty();
        dbItem.CreatedDate = DateTime.Now;
        dbItem.CreatorIpAddress = clientRequestAccessor.GetIpAddress();
        dbItem.RequestGuid = Guid.NewGuid();
        user.UserForgotPasswords.Add(dbItem);
        
        await unitOfWork.SaveChangesAsync();
        
        var emailArgs = new ResetPasswordNotificationEmailModel(user.Id)
        {
            UserForgotPasswordId = dbItem.Id
        };
        await schedulerService.EmailSynchronizedAsync(emailArgs);
    }
    
    public async Task<ValidatedValue<ForgotPasswordRequestModel>> CompleteAsync(CompleteForgotPasswordArgs args)
    {
        return await ValidatedValueFactory.CreateAsync(
            await completeForgotPasswordValidator.ValidateAsync(args), async () => await CompleteInnerAsync(args)
        );
    }

    private async Task<ForgotPasswordRequestModel> CompleteInnerAsync(CompleteForgotPasswordArgs args)
    {
        var dbItem = unitOfWork.Users.ForgotPasswords.GetRequest(args.RequestId);
        var model = new ForgotPasswordRequestModel
        {
            ErrorMessage = forgotPasswordQueryManager.GetRequestErrorMessage(dbItem)
        };
        
        if (string.IsNullOrWhiteSpace(model.ErrorMessage))
        {
            dbItem.ApprovedDateTime = DateTime.Now;
            dbItem.ApproverIpAddress = clientRequestAccessor.GetIpAddress();

            dbItem.User.Password = PasswordHash.HashPassword(args.NewPassword);
            dbItem.User.UpdatedDate = DateTime.Now;
            dbItem.User.UpdatedByUserId = dbItem.User.Id;

            await unitOfWork.SaveChangesAsync();
        }
        
        return model;
    }
}