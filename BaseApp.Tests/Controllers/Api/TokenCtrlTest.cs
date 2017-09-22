using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Projections.Users;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Controllers;
using BaseApp.Web.Controllers.Api;
using BaseApp.Web.Models.Api.Token;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.Controllers.Api
{
    [TestClass]
    public class TokenCtrlTest
    {
        [TestMethod]
        public void Retrieve_Success()
        {
            const string password = "correctpass";
            const string token = "sample_token";
            var logonMock = new Mock<ILogonManager>();
            var ctrlMock = ControllerTestFactory.CreateMock(new TokenController(logonMock.Object));
            var userRepositoryMock = ctrlMock.MockRepository(r => r.Users);

            var accountProj = new AccountProjection {Password = PasswordHash.HashPassword(password)};
            userRepositoryMock.Setup(x => x.GetAccountByLoginOrNull(It.IsAny<string>()))
                .Returns(accountProj);
            logonMock.Setup(x => x.GenerateToken(It.IsAny<LoggedClaims>(), It.IsAny<DateTime>())).Returns(token);
            
            var res = ctrlMock.Ctrl.Retrieve(new TokenRetrieveArgs {Password = password });
            Assert.AreEqual(true, res.ReturnValue.IsAuthenticated);
            Assert.AreEqual(token, res.ReturnValue.Token);
        }

        [TestMethod]
        public void Retrieve_User_Not_Found()
        {
            const string password = "correctpass";
            var logonMock = new Mock<ILogonManager>();
            var ctrlMock = ControllerTestFactory.CreateMock(new TokenController(logonMock.Object));
            var userRepositoryMock = ctrlMock.MockRepository(r => r.Users);
            
            userRepositoryMock.Setup(x => x.GetAccountByLoginOrNull(It.IsAny<string>()))
                .Returns((AccountProjection)null);

            var res = ctrlMock.Ctrl.Retrieve(new TokenRetrieveArgs { Password = password });
            Assert.AreEqual(false, res.ReturnValue.IsAuthenticated);
        }
    }
}
