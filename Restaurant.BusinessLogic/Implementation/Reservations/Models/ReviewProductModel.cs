using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations
{
	public class ReviewProductModel
	{
        public Guid Id { get; set; } 
		public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string Text { get; set; }
        
        public int Rating { get; set; }

        public IFormFile Picture {  get; set; }

        public byte[] ProductPicture { get; set; }
    }
}
