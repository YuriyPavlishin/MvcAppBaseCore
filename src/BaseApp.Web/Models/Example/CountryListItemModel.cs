using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseApp.Common.Utils;
using BaseApp.Data.Models;

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

    public class CountryListModel
    {
        public CountryArgsModel Args { get; set; }
        public List<CountryListItemModel> Items { get; set; }

        public CountryListModel(CountryArgsModel args, List<CountryListItemModel> items)
        {
            Args = args;
            Items = items;
        }
    }

    public class CountryListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Numeric Code")]
        public int? NumericCode { get; set; }
        public string Alpha2 { get; set; }
        public string Alpha3 { get; set; }
    }
}