using Restaurant.Common.DTOs;
using Restaurant.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products.Models
{
    public class FilterProductModel : FilterItemModel
    {
        public int? MaxPrice { get; set; } = null;
        public int? MaxPriceFilter { get; set; } = null;
        public int? MinPrice { get; set; } = 0;
        public int? SubcategoryId { get; set; } = (int)SubcategoryTypes.All;
    }
}
