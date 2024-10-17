using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApp.Common.Extensions;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Tests.Utils;
using BaseApp.Web.Code.BLL.Site.Examples;
using BaseApp.Web.Code.BLL.Site.Examples.Models;
using BaseApp.Web.Components.Example;
using BaseApp.Web.Models.Example;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BaseApp.Tests.Components
{
    [TestClass]
    public class CountryListViewCompTest
    {
        [TestMethod]
        public async Task Invoke_Success()
        {
            var args = new CountryArgsModel {Search = "MySearch"};
            var countryListItemModels = new List<CountryListItemModel>();
            
            var mockExample = new Mock<IExampleQueryManager>();
            mockExample.Setup(x => x.GetListAsync(It.IsAny<GetCountriesArgs>())).ReturnsAsync(() => new CountryListModel {Items = countryListItemModels});
            
            var compMock = ViewComponentTestFactory.CreateMock(new CountryListViewComponent(mockExample.Object));
            
            var res = (ViewViewComponentResult)await compMock.Comp.InvokeAsync(args);
            var model = (CountryListViewModel) res.ViewData!.Model;

            mockExample.Verify(x => x.GetListAsync(It.Is<GetCountriesArgs>(a => a.Query.EqualsIgnoreCase(args.Search))), Times.Once);
            Assert.AreEqual(countryListItemModels, model!.Items);
            Assert.AreEqual(args.Search, model.Args.Search);
        }
    }
}
