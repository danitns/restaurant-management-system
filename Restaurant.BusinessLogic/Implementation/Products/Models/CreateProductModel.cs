using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products.Models;

public class CreateProductModel
{
    public string Name { get; set; } 
    public int SubcategoryId { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Picture { get; set; }
}
