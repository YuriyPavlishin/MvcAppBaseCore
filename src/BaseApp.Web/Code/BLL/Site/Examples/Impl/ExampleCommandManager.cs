using System.IO;
using System.Threading.Tasks;
using BaseApp.Data.Files;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.BLL.Site.Examples.Models;
using Microsoft.AspNetCore.Http;

namespace BaseApp.Web.Code.BLL.Site.Examples.Impl;

public class ExampleCommandManager(IUnitOfWork unitOfWork, IAttachmentService attachmentService) : IExampleCommandManager
{
    public async Task<ExampleUserModel> SaveTestFileDataAsync(ExamplePostFileArgs args)
    {
        var result = new ExampleUserModel { Id = 1, Name = $"SaveTestFileData {args.UserId} company: {args.CompanyId}, file length: {args.CompanyFile.Length}" };
        var bytes = await ReadBytesAsync(args.CompanyFile);

        using var tran = unitOfWork.BeginTransaction();
        attachmentService.CreateAttachment(unitOfWork, args.UserId, args.CompanyFile.FileName, bytes, tran);
        await unitOfWork.SaveChangesAsync();
        tran.Commit();

        return result;
    }

    private static async Task<byte[]> ReadBytesAsync(IFormFile formFile)
    {
        using var ms = new MemoryStream();
        await formFile.CopyToAsync(ms);
        return ms.ToArray();
    }
}