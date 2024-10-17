using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.BLL.Admin.Users.Models;

public class GetUsersAdminArgs
{
    public string Query { get; set; }
    public PagingSortingArgs PagingSorting { get; set; }
}