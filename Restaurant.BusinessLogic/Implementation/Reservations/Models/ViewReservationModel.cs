using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations
{
	public class ViewReservationModel
	{
		public byte[] RestaurantPicture { get; set; }

		public string RestaurantName { get; set; }

		public string Address { get; set; }

		public DateTime Date { get; set; }
	}
}
