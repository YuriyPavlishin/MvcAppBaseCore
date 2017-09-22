using System.Collections.Generic;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Tests.Utils;
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
        public void Invoke_Success()
        {
            var compMock = ViewComponentTestFactory.CreateMock(new CountryListViewComponent());
            compMock.MockRepository(r => r.Countries);

            var countryListItemModels = new List<CountryListItemModel>();
            var args = new CountryArgsModel {Search = "MySearch"};
            compMock.Mapper.Setup(x => x.Map<List<CountryListItemModel>>(It.IsAny<List<Country>>()))
                .Returns((List<Country> source) => countryListItemModels);

            var res = (ViewViewComponentResult)compMock.Comp.Invoke(args);
            var model = (CountryListModel) res.ViewData.Model;

            Assert.AreEqual(countryListItemModels, model.Items);
            Assert.AreEqual(args.Search, model.Args.Search);
        }
    }
}
