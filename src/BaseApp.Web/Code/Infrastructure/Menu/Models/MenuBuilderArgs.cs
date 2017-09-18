using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;

namespace BaseApp.Web.Code.Infrastructure.Menu.Models
{
    public class MenuBuilderArgs
    {
        public MenuMvcArgs MenuMvcArgs { get; }
        public IUnitOfWork UnitOfWork { get; }
        public ILoggedUserAccessor LoggedUser { get; }

        public MenuBuilderArgs(MenuMvcArgs menuMvcArgs, IUnitOfWork unitOfWork, ILoggedUserAccessor loggedUser)
        {
            MenuMvcArgs = menuMvcArgs;
            UnitOfWork = unitOfWork;
            LoggedUser = loggedUser;
        }
    }
}
