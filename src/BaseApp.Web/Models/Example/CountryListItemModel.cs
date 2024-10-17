using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Common.Utils;
using BaseApp.Data.Models;
using BaseApp.Web.Code.BLL.Site.Examples.Models;

namespace BaseApp.Web.Models.Example
{
    public class CountryArgsModel
    {
        public string Search { get; set; }
        public PagingSortingInfo PagingSortingInfo { get; set; }

        public CountryArgsModel()
        {
            PagingSortingInfo = PagingSortingInfo.GetDefault(
                PropertyUtil.GetName<CountryListItemModel>(x=>x.Name)
            );
        }
    }

    public class CountryListViewModel
    {
        public CountryArgsModel Args { get; set; }
        public List<CountryListItemModel> Items { get; set; }

        public CountryListViewModel(CountryArgsModel args, List<CountryListItemModel> items)
        {
            Args = args;
            Items = items;
        }
    }
}