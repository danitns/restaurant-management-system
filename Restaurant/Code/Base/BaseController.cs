using Microsoft.AspNetCore.Mvc;
using Restaurant.Common.DTOs;

namespace Restaurant.Web.Code.Base
{
    public class BaseController : Controller
    {
        protected readonly CurrentUserDTO CurrentUser;

        public BaseController(ControllerDependencies dependencies)
            : base()
        {
            CurrentUser = dependencies.CurrentUser;
        }
    }
}
