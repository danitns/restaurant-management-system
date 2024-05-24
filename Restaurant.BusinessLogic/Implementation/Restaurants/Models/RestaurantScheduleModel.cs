using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
    public class RestaurantScheduleModel
    {
        public int DayOfWeek { get; set; } = 0;

        public TimeOnly OpeningTime { get; set; } = new TimeOnly(9, 0);

        public TimeOnly ClosingTime { get; set; } = new TimeOnly(22, 0);
    }
}
