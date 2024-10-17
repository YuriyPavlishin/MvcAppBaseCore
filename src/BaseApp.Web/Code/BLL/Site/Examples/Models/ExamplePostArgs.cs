using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BaseApp.Web.Code.BLL.Site.Examples.Models
{
    public class ExamplePostArgs
    {
        public int UserId { get; set; }
        public ExampleTestUserType UserType { get; set; }
        public int? CompanyId { get; set; }
        public List<ExampleUserModel> ExternalUsers { get; set; }
    }

    public class ExamplePostFileArgs
    {
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public IFormFile CompanyFile { get; set; }
    }

    public enum ExampleTestUserType
    {
        Test1Type = 1,
        Test2Type = 2,
        Test3Type = 3
    }
}
