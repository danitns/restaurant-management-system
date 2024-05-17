using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Products.Models;
using Restaurant.BusinessLogic.Implementation.Products.Validations;
using Restaurant.Common.Exceptions;
using Restaurant.Common.Extensions;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Products;

public class ProductService : BaseService
{
    private readonly CreateProductValidator CreateProductValidator;
    public ProductService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
    {
        CreateProductValidator = new CreateProductValidator(UnitOfWork);
    }

    public async Task CreateProduct(CreateProductModel model)
    {
        CreateProductValidator.Validate(model).ThenThrow(model);

        var product = Mapper.Map<CreateProductModel, Product>(model);

        if(model.Picture != null)
        {
            using (var ms = new MemoryStream())
            {
                model.Picture.CopyTo(ms);
                var fileBytes = ms.ToArray();
                product.Picture = fileBytes;
            }
        }
        
        UnitOfWork.Products.Insert(product);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ViewProductModel>> GetProducts(Guid restaurantId)
    {
        var products = await UnitOfWork.Products
            .Get()
            .Include(p => p.Restaurant)
            .Where(p => p.Restaurant.Id == restaurantId)
            .ToListAsync();
        return Mapper.Map<IEnumerable<Product>, IEnumerable<ViewProductModel>>(products);
    }
}
