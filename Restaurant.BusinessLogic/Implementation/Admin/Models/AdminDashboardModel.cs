using Restaurant.BusinessLogic.Implementation.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Admin 
{ 
	public class AdminDashboardModel
	{
        public List<DetailsUserModel> PendingManagers { get; set; } = new List<DetailsUserModel>();
    }
}
