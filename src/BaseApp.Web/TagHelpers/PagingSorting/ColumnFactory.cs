using System;
using System.Linq.Expressions;
using BaseApp.Common.Utils;
using BaseApp.Data.Models;
using BaseApp.Web.Code;

namespace BaseApp.Web.TagHelpers.PagingSorting
{
    public class ColumnFactory<TListItemModel>
    {
        public ColumnModel Create(Expression<Func<TListItemModel, object>> propertyExpr
            , string sortMemberName = null
            , string displayName = null)
        {
            return new ColumnModel
            {
                SortMemberName = sortMemberName ?? PropertyUtil.GetName(propertyExpr),
                DisplayName = displayName ?? ModelHelper.GetDisplayName(propertyExpr)
            };
        }
    }

    public class ColumnModel
    {
        public string SortMemberName { get; set; }
        public string DisplayName { get; set; }
    }

    public class SortColumnModel
    {
        public ColumnModel Column { get; set; }

        public bool SortedByCurrentField { get; set; }
        public bool SortByDescending { get; set; }

        public static SortColumnModel Create(ColumnModel column, PagingSortingInfo pagingSortingInfo)
        {
            var sortedByCurrentField = column.SortMemberName == pagingSortingInfo.SortMember;
            var sortByDescending = sortedByCurrentField && !pagingSortingInfo.SortDescending;

            return new SortColumnModel
            {
                Column = column,
                SortedByCurrentField = sortedByCurrentField,
                SortByDescending = sortByDescending
            };
        }
    }
}
