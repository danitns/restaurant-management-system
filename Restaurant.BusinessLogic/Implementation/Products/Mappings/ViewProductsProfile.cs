using AutoMapper;
using Restaurant.BusinessLogic.Implementation.Products.Models;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products.Mappings
{
    public class ViewProductsProfile : Profile
    {
        public ViewProductsProfile()
        {
            CreateMap<Product, ViewProductModel>();
                /*.ForMember(a => a.Picture, a => a.MapFrom(s => s.Picture))
                .ForMember(a => a.Subcategory, a => a.MapFrom(s => s.Subcategory));*/
        }
    }
}
