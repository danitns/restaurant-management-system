using Microsoft.AspNetCore.Http;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
	public class CreateRestaurantModel
	{
		public string Name { get; set; } = null!;

		public IFormFile Picture { get; set; } = null!;

		public string Address { get; set; } = null!;

		public int CityId { get; set; }
	}
}
