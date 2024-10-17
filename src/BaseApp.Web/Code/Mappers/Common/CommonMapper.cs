using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Models;
using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.Mappers.Common;

public class CommonMapper : MapperBase
{
    public CommonMapper()
    {
        CreateMap<PagingSortingInfo, PagingSortingArgs>();
        CreateMap<PagingSortingArgs, PagingSortingInfo>()
            .Ignore(x => x.TotalItemCount);
        CreateMap<PagingSortingInfo, PagingSortingDataModel>();
        
        CreateMap<Role, DataItemModel>();
    }
}