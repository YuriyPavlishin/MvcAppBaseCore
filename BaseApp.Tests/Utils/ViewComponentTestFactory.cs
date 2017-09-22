using System;
using System.Linq.Expressions;
using AutoMapper;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace BaseApp.Tests.Utils
{
    public class ViewComponentTestFactory
    {
        public static ViewComponentMockModel<T> CreateMock<T>(T component) where T : ViewComponent
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var compViewEngine = new Mock<ICompositeViewEngine>();
            var tempDataMock = new Mock<ITempDataDictionaryFactory>();
            var urlHelperFactoryMock = new Mock<IUrlHelperFactory>();
            var mapperMock = new Mock<IMapper>();
            var loggedUserAccessorMock = new Mock<ILoggedUserAccessor>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(tempDataMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactoryMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(ICompositeViewEngine)))
                .Returns(compViewEngine.Object);

            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUnitOfWork)))
                .Returns(unitOfWorkMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IMapper)))
                .Returns(mapperMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(ILoggedUserAccessor)))
                .Returns(loggedUserAccessorMock.Object);

            component.ViewComponentContext.ViewContext.HttpContext = new DefaultHttpContext();
            component.ViewComponentContext.ViewContext.HttpContext.RequestServices = serviceProviderMock.Object;
            //component.HttpContext.RequestServices = serviceProviderMock.Object;


            return new ViewComponentMockModel<T>
                   {
                       Comp = component,
                       ServiceProvider = serviceProviderMock,
                       UnitOfWork = unitOfWorkMock,
                       Mapper = mapperMock,
                       LoggedUserAccessor = loggedUserAccessorMock
                   };
        }
    }

    public class ViewComponentMockModel<T> where T : ViewComponent
    {
        public T Comp { get; set; }
        public Mock<IServiceProvider> ServiceProvider { get; set; }
        public Mock<IUnitOfWork> UnitOfWork { get; set; }
        public Mock<IMapper> Mapper { get; set; }
        public Mock<ILoggedUserAccessor> LoggedUserAccessor { get; set; }

        public Mock<R> MockRepository<R>(Expression<Func<IUnitOfWork, R>> repExpression) where R : class
        {
            var userValidatorMock = new Mock<R>();
            UnitOfWork.SetupGet(repExpression).Returns(userValidatorMock.Object);
            return userValidatorMock;
        }
    }
}
