using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Code.BLL.Site.Examples.Models;

namespace BaseApp.Web.Code.Mappers.Site
{
    public class ExampleMapper: MapperBase
    {
        public ExampleMapper()
        {
            CreateMap<Country, CountryListItemModel>();
        }
    }
}
