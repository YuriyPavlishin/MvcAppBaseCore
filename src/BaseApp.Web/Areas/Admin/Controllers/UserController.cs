using System;
using System.Linq;
using System.Threading;
using BaseApp.Common.Utils;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Web.Areas.Admin.Models.User;
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

        public IActionResult Edit(int? id)
        {
            UserEditModel model;
            if (id != null)
            {
                var user = UnitOfWork.Users.GetWithRolesOrNull(id.Value);
                if (user == null)
                    return NotFoundAction();

                model = Mapper.Map<UserEditModel>(user);
            }
            else
            {
                model = new UserEditModel();
            }

            
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user;
            if (model.Id != null)
            {
                user = UnitOfWork.Users.GetWithRolesOrNull(model.Id.Value);
            }
            else
            {
                user = UnitOfWork.Users.CreateEmpty();
                model.Password = PasswordHash.HashPassword(model.Password);
            }

            user = Mapper.Map(model, user);
            user.UpdatedByUserId = LoggedUser.Id;
            user.UpdatedDate = DateTime.Now;
            
            user.UserRoles.RemoveAll(x => !model.Roles.Contains(x.RoleId));
            foreach (var role in model.Roles.Where(x=>user.UserRoles.All(ur=>ur.RoleId != x)).ToList())
            {
                user.UserRoles.Add(new UserRole { RoleId = role });
            }

            UnitOfWork.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public void Delete(int id)
        {
            User user = UnitOfWork.Users.Get(id);
            user.DeletedDate = DateTime.Now;
            user.DeletedByUserId = LoggedUser.Id;

            UnitOfWork.SaveChanges();
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
