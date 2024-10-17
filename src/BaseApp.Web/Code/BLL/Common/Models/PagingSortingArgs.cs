namespace BaseApp.Web.Code.BLL.Common.Models;

public class PagingSortingArgs
{
    public int Page { get; set; }
    public int? PageSize { get; set; }
    public string SortMember { get; set; }
    public bool SortDescending { get; set; }
    public bool SkipTotalItemCountCalculation { get; set; }
}

public class PagingSortingDataModel
{
    public int? TotalItemCount { get; set; }
}