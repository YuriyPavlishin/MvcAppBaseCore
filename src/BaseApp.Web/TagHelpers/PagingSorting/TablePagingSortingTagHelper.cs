using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using BaseApp.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BaseApp.Web.TagHelpers.PagingSorting
{
    [HtmlTargetElement(Attributes = AppPagingSorting)]
    public class TablePagingSortingTagHelper: BasePagingSortingTagHelper
    {
        private const string AppPagingSorting = "app-paging-sorting";
        
        [HtmlAttributeName(AppPagingSorting)]
        public PagingSortingInfo PagingSorting { get; set; }
        public object AppFilterArgs { get; set; }
        public string AppNoRecordsFound { get; set; }

        private readonly HtmlHelper _htmlHelper;

        public TablePagingSortingTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper as HtmlHelper;
        }

        [ViewContext]
        public ViewContext ViewContext
        {
            set
            {
                _htmlHelper.Contextualize(value);
            }
        }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (PagingSorting == null)
                throw new ArgumentNullException(AppPagingSorting);
            
            SetPagingSorting(context, PagingSorting);

            var oldClassValue = (output.Attributes["class"] != null ? output.Attributes["class"].Value : "");
            output.Attributes.SetAttribute("class", oldClassValue + " paging-sorting");

            if ((PagingSorting.TotalItemCount ?? 0) <= 0)
            {
                output.SuppressOutput();
            }

            var argsJson = JsonSerializer.Serialize(AppFilterArgs);
            var argsStoreElement = $"<input type='hidden' data-paging-sorting-state='{argsJson}' />";

            var paginationOutput = (PagingSorting.TotalItemCount ?? 0) > 0 
                ? await GetPaginationOutput()
                : "";
            var noRecordsFoundOutput = (PagingSorting.TotalItemCount ?? 0) <= 0
                ? $"<div class=\"col-xs-12 paging-sorting-footer\" style=\"margin-top: 0\">{AppNoRecordsFound?? "No items were found"}</div>"
                : "";


            output.PostElement.SetHtmlContent(paginationOutput + " " + noRecordsFoundOutput + " " + argsStoreElement);
        }

        private async Task<string> GetPaginationOutput()
        {
            var partial = await _htmlHelper.PartialAsync("TagHelpers/PagingSorting/Pagination", PagingSorting);
            using (var paginationWriter = new StringWriter())
            {
                partial.WriteTo(paginationWriter, HtmlEncoder.Default);
                return paginationWriter.ToString();
            }
        }
    }
}
