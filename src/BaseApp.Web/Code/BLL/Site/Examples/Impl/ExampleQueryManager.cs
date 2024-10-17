using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.BLL.Site.Examples.Models;

namespace BaseApp.Web.Code.BLL.Site.Examples.Impl;

public class ExampleQueryManager(IUnitOfWork unitOfWork, IMapper mapper) : IExampleQueryManager
{
    public async Task<CountryListModel> GetListAsync(GetCountriesArgs args)
    {
        var pagingSorting = mapper.Map<PagingSortingInfo>(args.PagingSorting);
        var items = mapper.Map<List<CountryListItemModel>>(
            await unitOfWork.Countries.GetCountriesAsync(args.Query, pagingSorting)
        );
        return new CountryListModel
        {
            Items = items,
            PagingSortingData = mapper.Map<PagingSortingDataModel>(pagingSorting),
            PagingSortingInfo = pagingSorting
        };
    }
}