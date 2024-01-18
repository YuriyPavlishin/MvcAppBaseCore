using System;
using System.Threading.Tasks;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.BLL.ForgotPassword;
using BaseApp.Web.Code.BLL.ForgotPassword.Models;
using BaseApp.Web.Code.Infrastructure.Validation;
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
            var commandManagerMock = new Mock<IForgotPasswordCommandManager>();
            commandManagerMock.Setup(x => x.RequestAsync(It.IsAny<RequestForgotPasswordArgs>()))
                .ReturnsAsync(new ValidatedValue(Array.Empty<ValidationItemModel>()));
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(commandManagerMock.Object));
            const string email = "test@example.com";
            var res = await ctrlMock.Ctrl.Index(new ForgotPasswordModel { Email = email });

            commandManagerMock.Verify(x => x.RequestAsync(It.Is<RequestForgotPasswordArgs>(args => args.Email.Equals(email))), Times.Once);
            var redirectRes = (RedirectToActionResult)res;
            Assert.AreEqual(redirectRes.ActionName, "Success");
        }

        [TestMethod]
        public async Task Index_Post_Invalid_Model_State()
        {
            var commandManagerMock = new Mock<IForgotPasswordCommandManager>();
            commandManagerMock.Setup(x => x.RequestAsync(It.IsAny<RequestForgotPasswordArgs>()))
                .ReturnsAsync(new ValidatedValue([new ValidationItemModel {PropertyName = "Test", ErrorMessage = "Test"}]));
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(commandManagerMock.Object));
            var forgotPasswordModel = new ForgotPasswordModel();
            var res = await ctrlMock.Ctrl.Index(forgotPasswordModel);
            
            var redirectRes = (ViewResult)res;
            Assert.AreEqual(forgotPasswordModel, redirectRes.ViewData.Model);
        }


        [TestMethod]
        public void CompleteResetPassword_Error_Message()
        {
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(new Mock<IForgotPasswordCommandManager>().Object));
            var queryManager = new Mock<IForgotPasswordQueryManager>();
            const string errorMessage = "Lorem ipsum";
            queryManager.Setup(x => x.GetRequest(It.IsAny<GetRequestForgotPasswordArgs>()))
                .Returns(() => new ForgotPasswordRequestModel {ErrorMessage = errorMessage});
            var viewResult = (ViewResult)ctrlMock.Ctrl.CompleteResetPassword(It.IsAny<Guid>(), queryManager.Object);
            
            queryManager.Verify(x => x.GetRequest(It.IsAny<GetRequestForgotPasswordArgs>()), Times.Once);
            Assert.AreEqual("CompleteResetPasswordError", viewResult.ViewName);
            Assert.AreEqual(errorMessage, viewResult.ViewData.Model);
        }
        
        [TestMethod]
        public void CompleteResetPassword_Success()
        {
            var ctrlMock = ControllerTestFactory.CreateMock(new ForgotPasswordController(new Mock<IForgotPasswordCommandManager>().Object));
            var queryManager = new Mock<IForgotPasswordQueryManager>();
            
            var reqId = Guid.NewGuid();
            queryManager.Setup(x => x.GetRequest(It.IsAny<GetRequestForgotPasswordArgs>()))
                .Returns(() => new ForgotPasswordRequestModel {ErrorMessage = null});
            var viewResult = (ViewResult)ctrlMock.Ctrl.CompleteResetPassword(reqId, queryManager.Object);
            
            queryManager.Verify(x => x.GetRequest(It.IsAny<GetRequestForgotPasswordArgs>()), Times.Once);
            Assert.AreEqual(null, viewResult.ViewName);
            Assert.AreEqual(reqId, ((CompleteResetPasswordModel)viewResult.ViewData.Model).RequestId);
        }
    }
}
