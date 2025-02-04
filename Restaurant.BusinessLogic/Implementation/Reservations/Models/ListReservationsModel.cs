using Restaurant.BusinessLogic.Implementation.Reservations.Models;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations
{
	public class ListReservationsModel
	{
        public List<ViewReservationModel> Reservations { get; set; }

        public List<ViewUserBenefitModel> MyBenefits { get; set; }
    }
}
