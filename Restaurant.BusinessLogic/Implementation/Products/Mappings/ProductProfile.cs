using AutoMapper;
using Restaurant.BusinessLogic.Implementation.Products.Models;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductModel, Product>()
            .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
            .ForMember(a => a.Picture, a => a.Ignore());
    }
}
