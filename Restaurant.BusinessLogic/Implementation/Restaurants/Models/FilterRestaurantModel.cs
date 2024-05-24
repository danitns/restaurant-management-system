using Restaurant.Common.DTOs;
using Restaurant.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
    public class FilterRestaurantModel : FilterItemModel
    {
        public int? CityId { get; set; } = (int)Cities.All;
        public int? TypeId {  get; set; } = (int)RestaurantTypeEnum.All;
    }
}
