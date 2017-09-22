using System;
using System.Linq.Expressions;
using AutoMapper;
using BaseApp.Data.DataRepository.Users;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace BaseApp.Tests.Utils
{
    public class ControllerTestFactory
    {
        public static ControllerMockModel<T> CreateMock<T>(T controller) where T : Controller
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var tempDataMock = new Mock<ITempDataDictionaryFactory>();
            var urlHelperFactoryMock = new Mock<IUrlHelperFactory>();
            var mapperMock = new Mock<IMapper>();
            var loggedUserAccessorMock = new Mock<ILoggedUserAccessor>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(tempDataMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactoryMock.Object);

            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUnitOfWork)))
                .Returns(unitOfWorkMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IMapper)))
                .Returns(mapperMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(ILoggedUserAccessor)))
                .Returns(loggedUserAccessorMock.Object);

            controller.ControllerContext.HttpContext =
                new DefaultHttpContext { RequestServices = serviceProviderMock.Object };


            return new ControllerMockModel<T>
                   {
                       Ctrl = controller,
                       ServiceProvider = serviceProviderMock,
                       UnitOfWork = unitOfWorkMock,
                       Mapper = mapperMock,
                       LoggedUserAccessor = loggedUserAccessorMock
                   };
        }
    }

    public class ControllerMockModel<T> where T : Controller
    {
        public T Ctrl { get; set; }
        public Mock<IServiceProvider> ServiceProvider { get; set; }
        public Mock<IUnitOfWork> UnitOfWork { get; set; }
        public Mock<IMapper> Mapper { get; set; }
        public Mock<ILoggedUserAccessor> LoggedUserAccessor { get; set; }

        public Mock<R> MockRepository<R>(Expression<Func<IUnitOfWork, R>> repExpression) where R: class 
        {
            var userValidatorMock = new Mock<R>();
            UnitOfWork.SetupGet(repExpression).Returns(userValidatorMock.Object);
            return userValidatorMock;
        }
    }
}
