using Restaurant.BusinessLogic.Implementation.Tables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
    public class ViewRestaurantModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public byte[] Picture { get; set; } = null!;

        public string Address { get; set; } = null!;

		public string Description { get; set; } = null!;

		public string CityName { get; set; }

        public string TypeName {  get; set; }

        public bool IsMyRestaurant { get; set; }

        public List<RestaurantScheduleModel> Schedules { get; set; } = new();

        public List<TableModel> Tables { get; set; } = new List<TableModel>();
    }
}
