using System.Collections.Generic;
using BaseApp.Web.Code.Infrastructure;
using BaseApp.Web.Models.Example;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.BLL.Site.Examples;
using BaseApp.Web.Code.BLL.Site.Examples.Models;

namespace BaseApp.Web.Components.Example
{
    public class CountryListViewComponent(IExampleQueryManager queryManager): ViewComponentBase
    {
        public async Task<IViewComponentResult> InvokeAsync(CountryArgsModel args)
        {
            var result = await queryManager.GetListAsync(new GetCountriesArgs
            {
                Query = args.Search,
                PagingSorting = Mapper.Map<PagingSortingArgs>(args.PagingSortingInfo)
            });
            args.PagingSortingInfo = result.PagingSortingInfo;

            return View(new CountryListViewModel(args, result.Items));
        }
    }
}
