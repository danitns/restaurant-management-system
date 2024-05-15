using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations
{
    public class CreateReservationModel
    {
        public string? Phone { get; set; }

        public int NumberOfGuests {  get; set; }

        public DateOnly Date {  get; set; }

        public string Time { get; set; }
		public string RestaurantName { get; set; }
	}
}
