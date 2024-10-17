using System;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Impl;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.BLL.Site.ForgotPasswords;

[TestClass]
public class ForgotPasswordQueryManagerTest
{
    [TestMethod]
    public void CompleteResetPassword_ResetPassword_Success()
    {
        var result = ProcessCompleteResetPassword(() => new UserForgotPassword { CreatedDate = DateTime.Now });
        Assert.IsTrue(string.IsNullOrWhiteSpace(result.ErrorMessage));
    }
    
    [TestMethod]
    public void CompleteResetPassword_ResetPassword_UrlExpired()
    {
        var result = ProcessCompleteResetPassword(() => new UserForgotPassword { CreatedDate = DateTime.Now.AddDays(-1).AddHours(-1) });
        Assert.AreEqual(ForgotPasswordRequestModel.ExpiredMessage, result.ErrorMessage);
    }
    
    [TestMethod]
    public void CompleteResetPassword_ResetPassword_AlreadyApproved()
    {
        var result = ProcessCompleteResetPassword(() => new UserForgotPassword { ApprovedDateTime = DateTime.Now });
        Assert.AreEqual(ForgotPasswordRequestModel.ExpiredMessage, result.ErrorMessage);
    }
    
    [TestMethod]
    public void CompleteResetPassword_ResetPassword_KeyNotFound()
    {
        var result = ProcessCompleteResetPassword(() => null);
        Assert.AreEqual(ForgotPasswordRequestModel.NotFoundMessage, result.ErrorMessage);
    }
    
    private static ForgotPasswordRequestModel ProcessCompleteResetPassword(Func<UserForgotPassword> getForgotResult)
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var queryManager = new ForgotPasswordQueryManager(unitOfWorkMock.Object);
        var userRepositoryMock = unitOfWorkMock.MockRepository(uow => uow.Users);
        userRepositoryMock.Setup(repository => repository.ForgotPasswords.GetRequest(It.IsAny<Guid>())).Returns(getForgotResult);
        
        return queryManager.GetRequest(new GetRequestForgotPasswordArgs());
    }
}