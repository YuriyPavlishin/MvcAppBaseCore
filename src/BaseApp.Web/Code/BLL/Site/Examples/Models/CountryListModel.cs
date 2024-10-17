using System.Collections.Generic;
using System.Text.Json.Serialization;
using BaseApp.Data.Models;
using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.BLL.Site.Examples.Models;

public class CountryListModel
{
    public List<CountryListItemModel> Items { get; set; }
    public PagingSortingDataModel PagingSortingData { get; set; }
    [JsonIgnore]
    public PagingSortingInfo PagingSortingInfo { get; set; }
}