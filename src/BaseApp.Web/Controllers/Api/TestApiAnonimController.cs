using BaseApp.Data.Files;
using BaseApp.Web.Code.Infrastructure.Api;
using BaseApp.Web.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers.Api
{
    [AllowAnonymous]
    public class TestApiAnonimController: ControllerBaseApi
    {
        private readonly IAttachmentService _attachmentService;

        public TestApiAnonimController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ApiResult<UserModel> GetUserTest()
        {
            var result = new UserModel() { Id = 1, Name = $"teststring {LoggedUser.IdOrNull}" };
            return ApiResult.Success(result);
        }

        [HttpGet]
        public ApiResult<UserModel> GetForValidation(TestValidationArgs args)
        {
            var result = new UserModel() { Id = 1, Name = $"GetForValidation {args.UserId}" };
            return ApiResult.Success(result);
        }

        [HttpPost]
        public ApiResult<UserModel> SaveTestData(TestPostArgs args)
        {
            var result = new UserModel() { Id = 1, Name = $"SaveTestData {args.UserId} company: {args.CompanyId}, UserType: {args.UserType}" };
            return ApiResult.Success(result);
        }

        /// <summary>
        /// Sample test for attachment service (for test comment in swagger purpose)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<UserModel> SaveTestFileData(TestPostFileArgs args)
        {
            var result = new UserModel() { Id = 1, Name = $"SaveTestFileData {args.UserId} company: {args.CompanyId}, file length: {args.CompanyFile.Length}" };
            byte[] bytes = new byte[args.CompanyFile.Length];
            using (var stream = args.CompanyFile.OpenReadStream())
            {
                stream.Read(bytes, 0, (int)args.CompanyFile.Length);
            }

            using (var tran = UnitOfWork.BeginTransaction())
            {
                _attachmentService.CreateAttachment(UnitOfWork, args.UserId, args.CompanyFile.FileName, bytes, tran);
                UnitOfWork.SaveChanges();
                tran.Commit();
            }
            
            return ApiResult.Success(result);
        }
    }
}
