using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api
{
    public class TestApiController: ControllerBaseApi
    {
        [HttpPost]
        public ApiResult<UserModel> SaveTestData(TestPostArgs args)
        {
            var result = new UserModel() { Id = 1, Name = $"SaveTestData {args.UserId} company: {args.CompanyId}, UserType: {args.UserType}" };
            return ApiResult.Success(result);
        }
    }
}
