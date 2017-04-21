using System;
using BaseApp.Data.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BaseApp.Web.TagHelpers.PagingSorting
{
    public abstract class BasePagingSortingTagHelper : TagHelper
    {
        public void SetPagingSorting(TagHelperContext context, PagingSortingInfo pagingSorting)
        {
            context.Items.Add(typeof(PagingSortingInfo), pagingSorting);
        }

        public PagingSortingInfo GetPagingSorting(TagHelperContext context)
        {
            object result;
            if (context.Items.TryGetValue(typeof(PagingSortingInfo), out result))
                return (PagingSortingInfo)result;

            throw new ArgumentNullException(nameof(PagingSortingInfo));
        }
    }
}
