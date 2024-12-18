using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations
{
	public class ReviewReservationModel
	{
        public Guid ReservationId { get; set; }
		public string RestaurantName { get; set;}

        public string Text { get; set; }

        public int Rating { get; set; }

        public List<ReviewProductModel> ProductReviews { get; set; }
    }
}
