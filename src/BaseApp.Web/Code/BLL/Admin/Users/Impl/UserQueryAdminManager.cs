using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;

namespace BaseApp.Web.Code.BLL.Admin.Users.Impl;

public class UserQueryAdminManager(IUnitOfWork unitOfWork, IMapper mapper) : IUserQueryAdminManager
{
    public async Task<UserListAdminModel> GetListAsync(GetUsersAdminArgs args)
    {
        var pagingSorting = mapper.Map<PagingSortingInfo>(args.PagingSorting);
        var items = mapper.Map<List<UserListItemAdminModel>>(
            await unitOfWork.Users.GetUsersForAdminAsync(args.Query, pagingSorting)
        );
        return new UserListAdminModel
        {
            Items = items,
            PagingSortingData = mapper.Map<PagingSortingDataModel>(pagingSorting),
            PagingSortingInfo = pagingSorting
        };
    }

    public UserForEditAdminModel GetForEdit(GetByIdOptionalArgs args)
    {
        var model = args.Id == null ? new UserForEditAdminModel() : mapper.Map<UserForEditAdminModel>(unitOfWork.Users.Get(args.Id.Value));
        model.DictionaryRoles = mapper.Map<List<DataItemModel>>(unitOfWork.Users.Roles.GetAllRoles());
        return model;
    }
}