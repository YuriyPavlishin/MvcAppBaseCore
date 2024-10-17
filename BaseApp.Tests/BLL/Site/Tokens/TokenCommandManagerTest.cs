using System;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Projections.Users;
using BaseApp.Data.Infrastructure;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.BLL.Site.Tokens.Impl;
using BaseApp.Web.Code.BLL.Site.Tokens.Models;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.BLL.Site.Tokens
{
    [TestClass]
    public class TokenCommandManagerTest
    {
        [TestMethod]
        public void Retrieve_Success()
        {
            const string password = "correctpass";
            const string token = "sample_token";
            var logonMock = new Mock<ILogonManager>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = unitOfWorkMock.MockRepository(r => r.Users);
            var accountProj = new AccountProjection {Password = PasswordHash.HashPassword(password)};
            userRepositoryMock.Setup(x => x.GetAccountByLoginOrNull(It.IsAny<string>()))
                .Returns(accountProj);
            logonMock.Setup(x => x.GenerateToken(It.IsAny<LoggedClaims>(), It.IsAny<DateTime>())).Returns(token);
            
            var tokenCommandManager = new TokenCommandManager(unitOfWorkMock.Object, logonMock.Object);
            
            var res = tokenCommandManager.Retrieve(new RetrieveTokenArgs {Password = password });
            Assert.AreEqual(true, res.IsAuthenticated);
            Assert.AreEqual(token, res.Token);
        }

        [TestMethod]
        public void Retrieve_User_Not_Found()
        {
            const string password = "correctpass";
            var logonMock = new Mock<ILogonManager>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = unitOfWorkMock.MockRepository(r => r.Users);
            userRepositoryMock.Setup(x => x.GetAccountByLoginOrNull(It.IsAny<string>()))
                .Returns((AccountProjection)null);
            
            var tokenCommandManager = new TokenCommandManager(unitOfWorkMock.Object, logonMock.Object);

            var res = tokenCommandManager.Retrieve(new RetrieveTokenArgs { Password = password });
            Assert.AreEqual(false, res.IsAuthenticated);
        }
    }
}
