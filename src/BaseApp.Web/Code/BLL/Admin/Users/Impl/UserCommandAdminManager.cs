using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Code.Infrastructure.Validation;
using FluentValidation;

namespace BaseApp.Web.Code.BLL.Admin.Users.Impl;

public class UserCommandAdminManager(IUnitOfWork unitOfWork, ILoggedUserAccessor loggedUserAccessor, IMapper mapper
    , IValidator<EditUserAdminArgs> userValidator) : IUserCommandAdminManager
{
    public async Task<ValidatedValue<EditUserResultAdminModel>> EditAsync(EditUserAdminArgs args)
    {
        return await ValidatedValueFactory.CreateAsync(
            await userValidator.ValidateAsync(args), async () => await EditInnerAsync(args)
        );
    }

    private async Task<EditUserResultAdminModel> EditInnerAsync(EditUserAdminArgs args)
    {
        User dbItem;
        if (args.Id != null)
        {
            dbItem = unitOfWork.Users.GetWithRolesOrNull(args.Id.Value);
        }
        else
        {
            dbItem = unitOfWork.Users.CreateEmpty();
            dbItem.Password = PasswordHash.HashPassword(args.Password);
        }

        dbItem = mapper.Map(args, dbItem);
        dbItem.UpdatedByUserId = loggedUserAccessor.Id;
        dbItem.UpdatedDate = DateTime.Now;
            
        dbItem.UserRoles.RemoveAll(x => !args.Roles.Contains(x.RoleId));
        foreach (var role in args.Roles.Where(x=>dbItem.UserRoles.All(ur=>ur.RoleId != x)).ToList())
        {
            dbItem.UserRoles.Add(new UserRole { RoleId = role });
        }

        await unitOfWork.SaveChangesAsync();
        
        return new EditUserResultAdminModel {Id = dbItem.Id};
    }
    
    public async Task DeleteAsync(GetByIdArgs args)
    {
        var user = unitOfWork.Users.Get(args.Id);
        user.DeletedDate = DateTime.Now;
        user.DeletedByUserId = loggedUserAccessor.Id;

        await unitOfWork.SaveChangesAsync();
    }
}