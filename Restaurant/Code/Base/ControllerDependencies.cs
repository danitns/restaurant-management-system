using Restaurant.Common.DTOs;

namespace Restaurant.Web.Code.Base
{
    public class ControllerDependencies
    {
        public CurrentUserDTO CurrentUser { get; set; }

        public ControllerDependencies(CurrentUserDTO currentUser)
        {
            CurrentUser = currentUser;
        }
    }
}
