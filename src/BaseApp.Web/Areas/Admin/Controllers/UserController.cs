using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Areas.Admin.Models.User;
using BaseApp.Web.Code.BLL.Admin.Users;
using BaseApp.Web.Code.BLL.Admin.Users.Models;
using BaseApp.Web.Code.BLL.Common.Models;
using BaseApp.Web.Code.Infrastructure.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Areas.Admin.Controllers
{
    public class UserController : ControllerBaseAdminRequired
    {
        public IActionResult Index()
        {
            return View(new UserListArgs());
        }

        public IActionResult GetUsersList(UserListArgs args)
        {
            return ViewComponent("UserList", new { args = args } );
        }

        public IActionResult Edit(int? id, [FromServices] IUserQueryAdminManager queryManager)
        {
            return View(Mapper.Map<UserEditModel>(queryManager.GetForEdit(new GetByIdOptionalArgs { Id = id })));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel model, [FromServices] IUserCommandAdminManager commandManager)
        {
            var (isValid, _) = await ValidateAndPerform(
                async () => await commandManager.EditAsync(Mapper.Map<EditUserAdminArgs>(model)));
            if (isValid)
            {
                return RedirectToAction("Index", "User");
            }
            return View(model);
        }

        [HttpPost]
        public async Task Delete(int id, [FromServices] IUserCommandAdminManager commandManager)
        {
            await commandManager.DeleteAsync(new GetByIdArgs { Id = id });
        }

        //TODO: PLEASE REMOVE THIS METHOD (ONLY FOR TRANSACTION EXAMPLE USED)
        [HttpPost]
        public void RestoreDeletedUsers()
        {
            Thread.Sleep(3000);

            var users = UnitOfWork.Users.GetDeleted();

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                foreach (var user in users)
                {
                    user.DeletedDate = null;
                    user.DeletedByUserId = null;
                    UnitOfWork.SaveChanges();
                }

                transaction.Commit();
            }
        }
    }
}
