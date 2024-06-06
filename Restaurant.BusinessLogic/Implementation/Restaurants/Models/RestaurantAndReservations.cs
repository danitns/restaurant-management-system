using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
	public class RestaurantAndReservations
	{
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public int NumberOfReservations { get; set; }
    }
}
