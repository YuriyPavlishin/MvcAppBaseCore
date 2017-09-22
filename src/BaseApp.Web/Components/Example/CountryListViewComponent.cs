using System.Collections.Generic;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Models.Example;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseApp.Web.Components.Example
{
    public class CountryListViewComponent: ViewComponentBase
    {
        public IViewComponentResult Invoke(CountryArgsModel args)
        {
            var dbItems = UnitOfWork.Countries.GetCountries(args.Search, args.PagingSortingInfo);
            var countries = Mapper.Map<List<CountryListItemModel>>(dbItems);

            return View(new CountryListModel(args, countries));
        }
    }
}
