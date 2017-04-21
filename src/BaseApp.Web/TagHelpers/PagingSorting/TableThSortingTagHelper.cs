using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BaseApp.Web.TagHelpers.PagingSorting
{
    [HtmlTargetElement(Attributes = AppColumn)]
    public class TableThSortingTagHelper : BasePagingSortingTagHelper
    {
        private const string AppColumn = "app-column";

        [HtmlAttributeName(AppColumn)]
        public ColumnModel Column { get; set; }

        private readonly HtmlHelper _htmlHelper;

        public TableThSortingTagHelper(IHtmlHelper htmlHelper)
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
            if (Column == null)
                throw new ArgumentNullException(AppColumn);

            var pagingSorting = GetPagingSorting(context);
            var sortedColumn = SortColumnModel.Create(Column, pagingSorting);

            var oldClassValue = (output.Attributes["class"]!=null ? output.Attributes["class"].Value: "");
            output.Attributes.SetAttribute("class", oldClassValue + " paging-sorting-column");
            output.Attributes.SetAttribute("data-sortmember", sortedColumn.Column.SortMemberName);
            output.Attributes.SetAttribute("data-sortdescending", sortedColumn.SortByDescending.ToString());

            var partial = await _htmlHelper.PartialAsync("TagHelpers/PagingSorting/Header", sortedColumn);

            using (var writer = new StringWriter())
            {
                partial.WriteTo(writer, HtmlEncoder.Default);
                output.Content.SetHtmlContent(writer.ToString());
            }
        }
    }
}
