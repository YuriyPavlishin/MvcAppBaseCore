using System.Threading.Tasks;
using BaseApp.Web.Code.BLL.Site.Examples;
using BaseApp.Web.Code.BLL.Site.Examples.Models;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Code.Infrastructure.LogOn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api.Site
{
    public class ExampleController : ControllerBaseSiteApi
    {
        [HttpGet, AllowAnonymous]
        public async Task<ApiResult<CountryListModel>> GetList([FromServices] IExampleQueryManager queryManager, GetCountriesArgs args)
        {
            return await queryManager.GetListAsync(args);
        }
        
        [HttpPost]
        public ApiResult<ExampleUserModel> SaveTestData(ExamplePostArgs args)
        {
            return new ExampleUserModel { Id = 1, Name = $"SaveTestData {args.UserId} company: {args.CompanyId}, UserType: {args.UserType}" };
        }
        
        [HttpPost, AllowAnonymous]
        public ApiResult<ExampleUserModel> SaveTestDataAnonymous(ExamplePostArgs args)
        {
            return new ExampleUserModel { Id = 1, Name = $"SaveTestData {args.UserId} company: {args.CompanyId}, UserType: {args.UserType}" };
        }
        
        [HttpGet, AllowAnonymous]
        public ApiResult<ExampleUserModel> GetUserTest([FromServices] ILoggedUserAccessor loggedUserAccessor)
        {
            return new ExampleUserModel { Id = 1, Name = $"teststring {loggedUserAccessor.IdOrNull}" };
        }

        /// <summary>
        /// Sample test for attachment service (for test comment in swagger purpose)
        /// </summary>
        [HttpPost, AllowAnonymous]
        public async Task<ApiResult<ExampleUserModel>> SaveTestFileData(ExamplePostFileArgs args, [FromServices] IExampleCommandManager commandManager)
        {
            return await commandManager.SaveTestFileDataAsync(args);
        }
    }
}
