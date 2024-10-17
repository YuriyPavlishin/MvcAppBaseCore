using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.BLL.Site.Examples.Models;

public class GetCountriesArgs
{
    public string Query { get; set; }
    public PagingSortingArgs PagingSorting { get; set; }
}