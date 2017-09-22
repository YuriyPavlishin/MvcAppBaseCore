using BaseApp.Data.DataContext.Entities;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Controllers;
using BaseApp.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.Controllers
{
    [TestClass]
    public class AccountCtrlTest
    {
        [TestMethod]
        public void UserProfile_Success()
        {
            var loggedUserId = 4;
            var logonMock = new Mock<ILogonManager>();
            var ctrlMock = ControllerTestFactory.CreateMock(new AccountController(logonMock.Object));
            ctrlMock.LoggedUserAccessor.SetupGet(x => x.Id).Returns(loggedUserId);

            var userRepositoryMock = ctrlMock.MockRepository(uow => uow.Users);
            userRepositoryMock.Setup(r => r.GetWithRolesOrNull(It.IsAny<int>())).Returns(new User());

            var userProfileModel = new UserProfileModel();

            ctrlMock.Mapper.Setup(x => x.Map<UserProfileModel>(It.IsAny<User>()))
                .Returns((User source) => userProfileModel);

            var res = ctrlMock.Ctrl.UserProfile();

            var viewRes = (ViewResult)res;
            userRepositoryMock.Verify(r => r.GetWithRolesOrNull(loggedUserId), Times.Once);
            Assert.AreEqual(userProfileModel, viewRes.ViewData.Model);
        }
    }
}
