using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations.Models
{
	public class ViewUserBenefitModel
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public bool? IsActive { get; set; }
		public string Details { get; set; } = null!;
		public int Value { get; set; }
        public string RestaurantName { get; set; }
    }
}
