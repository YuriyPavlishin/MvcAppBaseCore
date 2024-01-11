using System;
using System.Threading.Tasks;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.Scheduler;
using BaseApp.Web.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using BaseApp.Web.Controllers;
using BaseApp.Web.Models.ForgotPassword;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.Controllers
{
    [TestClass]
    public class ForgotPasswordCtrlTest
    {
        [TestMethod]
        public async Task Index_Post_Success()
        {
            var scheduleMock = new Mock<ISchedulerService>();
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(scheduleMock.Object));

            var userRepositoryMock = ctrlMock.MockRepository(uow => uow.Users);
            userRepositoryMock.Setup(repository => repository.GetByEmailOrNull(It.IsAny<string>(), It.IsAny<bool>())).Returns(new User());

            var email = "test@example.com";
            var res = await ctrlMock.Ctrl.Index(new ForgotPasswordModel() { Email = email });

            userRepositoryMock.Verify(x => x.GetByEmailOrNull(email, false), Times.Once);
            ctrlMock.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            scheduleMock.Verify(x => x.EmailSynchronizedAsync(It.IsAny<ResetPasswordNotificationEmailModel>()), Times.Once);
            var redirRes = (RedirectToActionResult)res;
            Assert.AreEqual(redirRes.ActionName, "Success");
        }

        [TestMethod]
        public async Task Index_Post_Invalid_Model_State()
        {
            var scheduleMock = new Mock<ISchedulerService>();
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(scheduleMock.Object));
            ctrlMock.Ctrl.ModelState.AddModelError("Error", "Error");

            var email = "test@example.com";
            var forgotPasswordModel = new ForgotPasswordModel() { Email = email };
            var res = await ctrlMock.Ctrl.Index(forgotPasswordModel);

            ctrlMock.UnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
            scheduleMock.Verify(x => x.EmailSynchronizedAsync(It.IsAny<ResetPasswordNotificationEmailModel>()), Times.Never);
            var redirRes = (ViewResult)res;
            Assert.AreEqual(forgotPasswordModel, redirRes.ViewData.Model);
        }


        [TestMethod]
        public void CompleteResetPassword_ResetPasswordUrlExpired()
        {
            var viewResult = ProcessCompleteResetPassword(() => new UserForgotPassword { CreatedDate = DateTime.Now.AddDays(-1).AddHours(-1) });

            Assert.AreEqual("CompleteResetPasswordError", viewResult.ViewName);
            Assert.AreEqual("Reset Password url expired", viewResult.ViewData.Model);
        }

        [TestMethod]
        public void CompleteResetPassword_ResetPasswordKeyNotFound()
        {
            var viewResult = ProcessCompleteResetPassword(() => null);
            
            Assert.AreEqual("CompleteResetPasswordError", viewResult.ViewName);
            Assert.AreEqual("Reset Password key not found", viewResult.ViewData.Model);
        }

        [TestMethod]
        public void CompleteResetPassword_Success()
        {
            var reqId = Guid.NewGuid();
            var viewResult = ProcessCompleteResetPassword(() => new UserForgotPassword { CreatedDate = DateTime.Now.AddHours(-1) }, reqId);
           
            Assert.AreEqual(null, viewResult.ViewName);
            Assert.AreEqual(reqId, ((CompleteResetPasswordModel)viewResult.ViewData.Model).RequestId);
        }

        private static ViewResult ProcessCompleteResetPassword(Func<UserForgotPassword> getForgotResult, Guid? reqId = null)
        {
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(new Mock<ISchedulerService>().Object));

            var userRepositoryMock = ctrlMock.MockRepository(uow => uow.Users);
            userRepositoryMock.Setup(repository => repository.GetForgotPasswordRequest(It.IsAny<Guid>())).Returns(getForgotResult);

            var res = ctrlMock.Ctrl.CompleteResetPassword(reqId ?? Guid.NewGuid());
            var viewResult = (ViewResult)res;
            return viewResult;
        }
    }
}
