using System.ComponentModel.DataAnnotations;

namespace BaseApp.Web.Code.BLL.Site.Examples.Models;

public class CountryListItemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Display(Name = "Numeric Code")]
    public int? NumericCode { get; set; }
    public string Alpha2 { get; set; }
    public string Alpha3 { get; set; }
}