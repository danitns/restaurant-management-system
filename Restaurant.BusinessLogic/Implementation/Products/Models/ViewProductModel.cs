using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products.Models
{
    public class ViewProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public byte[] Picture { get; set; }

        public int SubcategoryId { get; set; }

        public Guid RestaurantId { get; set; }
    }
}
