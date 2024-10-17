using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataRepository.Users;
using BaseApp.Data.Infrastructure;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Impl;
using BaseApp.Web.Code.BLL.Site.ForgotPasswords.Models;
using BaseApp.Web.Code.Infrastructure.ClientRequests;
using BaseApp.Web.Code.Scheduler;
using BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.BLL.Site.ForgotPasswords;

[TestClass]
public class ForgotPasswordCommandManagerTest
{
    [TestMethod]
    public async Task Request_Success()
    {
        const string ipAddress = "example_ip";
        var commandManager = Create(out var mockData);
        mockData.ClientRequestAccessor.Setup(x => x.GetIpAddress()).Returns(ipAddress);
        var user = new User {Id = -1};
        mockData.UserRepository.Setup(r => r.GetByEmailOrNull(It.IsAny<string>(), It.IsAny<bool>())).Returns(user);
        var userForgotPassword = new UserForgotPassword();
        mockData.UserForgotPasswordRepository.Setup(r => r.CreateEmpty()).Returns(userForgotPassword);
        
        const string email = "test@example.com";
        await commandManager.RequestAsync(new RequestForgotPasswordArgs {Email = email});
        
        mockData.UserRepository.Verify(r => r.GetByEmailOrNull(It.Is(email, EqualityComparer<string>.Default), false), Times.Once);
        mockData.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.AreEqual(user.UserForgotPasswords.Single(), userForgotPassword);
        Assert.AreEqual(userForgotPassword.CreatorIpAddress, ipAddress);
        mockData.SchedulerService.Verify(x => x.EmailSynchronizedAsync(It.Is<ResetPasswordNotificationEmailModel>(m => m.CreatedByUserId == user.Id)), Times.Once);
    }

    [TestMethod]
    public async Task Complete_Error()
    {
        var commandManager = Create(out var mockData);
        const string errorMessage = "Error";
        mockData.ForgotPasswordQueryManager.Setup(x => x.GetRequestErrorMessage(It.IsAny<UserForgotPassword>())).Returns(errorMessage);
        
        var args = new CompleteForgotPasswordArgs {RequestId = Guid.NewGuid()};
        var result = await commandManager.CompleteAsync(args);
        
        mockData.UserForgotPasswordRepository.Verify(x => x.GetRequest(It.Is(args.RequestId, EqualityComparer<Guid>.Default)), Times.Once);
        mockData.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        Assert.AreEqual(errorMessage, result.EnsuredValue.ErrorMessage);
    }
    
    [TestMethod]
    public async Task Complete_Success()
    {
        var commandManager = Create(out var mockData);
        const string ipAddress = "example_ip";
        mockData.ForgotPasswordQueryManager.Setup(x => x.GetRequestErrorMessage(It.IsAny<UserForgotPassword>())).Returns("");
        mockData.ClientRequestAccessor.Setup(x => x.GetIpAddress()).Returns(ipAddress);
        var userForgotPassword = new UserForgotPassword { User = new User() };
        mockData.UserForgotPasswordRepository.Setup(x => x.GetRequest(It.IsAny<Guid>())).Returns(userForgotPassword);

        var args = new CompleteForgotPasswordArgs { RequestId = Guid.NewGuid(), NewPassword = "TestPassword" };
        await commandManager.CompleteAsync(args);
        
        mockData.UserForgotPasswordRepository.Verify(x => x.GetRequest(It.Is(args.RequestId, EqualityComparer<Guid>.Default)), Times.Once);
        mockData.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.IsTrue(PasswordHash.ValidatePassword(args.NewPassword, userForgotPassword.User.Password));
        Assert.AreEqual(ipAddress, userForgotPassword.ApproverIpAddress);
    }
    
    private static ForgotPasswordCommandManager Create(out ForgotPasswordCommandManagerMockData mockData)
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var userRepository = unitOfWork.MockRepository(uow => uow.Users);
        mockData = new ForgotPasswordCommandManagerMockData
        {
            UnitOfWork = unitOfWork,
            UserRepository = userRepository,
            UserForgotPasswordRepository = userRepository.MockRepository(uow => uow.ForgotPasswords),
            ForgotPasswordQueryManager = new Mock<IForgotPasswordQueryManager>(),
            SchedulerService = new Mock<ISchedulerService>(),
            ClientRequestAccessor = new Mock<IClientRequestAccessor>()
        };
        return new ForgotPasswordCommandManager(unitOfWork.Object
            , ValidatorTestFactory.CreateSuccessMock<RequestForgotPasswordArgs>().Object, ValidatorTestFactory.CreateSuccessMock<CompleteForgotPasswordArgs>().Object
            , mockData.ForgotPasswordQueryManager.Object, mockData.SchedulerService.Object, mockData.ClientRequestAccessor.Object);
    }
    
    private class ForgotPasswordCommandManagerMockData
    {
        public Mock<IUnitOfWork> UnitOfWork { get; set; }
        public Mock<IUserRepository> UserRepository { get; set; }
        public Mock<IUserForgotPasswordRepository> UserForgotPasswordRepository { get; set; }
        public Mock<IForgotPasswordQueryManager> ForgotPasswordQueryManager { get; set; }
        public Mock<ISchedulerService> SchedulerService { get; set; }
        public Mock<IClientRequestAccessor> ClientRequestAccessor { get; set; }
    }
}