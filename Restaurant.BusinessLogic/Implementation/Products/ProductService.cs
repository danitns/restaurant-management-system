using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Products.Models;
using Restaurant.BusinessLogic.Implementation.Products.Validations;
using Restaurant.BusinessLogic.Implementation.Restaurants;
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
    static private FilterProductModel FilterModel;

    private readonly CreateProductValidator CreateProductValidator;
    public ProductService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
    {
        CreateProductValidator = new CreateProductValidator(UnitOfWork);

        if (FilterModel == null)
        {
            FilterModel = new FilterProductModel();
        }
    }

    public async Task CreateProduct(CreateProductModel model)
    {
        CreateProductValidator.Validate(model).ThenThrow(model);

        var product = Mapper.Map<CreateProductModel, Product>(model);

        if (model.Picture != null)
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

    public async Task<IEnumerable<ViewProductModel>> GetProducts(Guid restaurantId, FilterProductModel? filterModel)
    {
        var productsQuery = UnitOfWork.Products
                .Get()
                .Where(p => p.RestaurantId == restaurantId);

        if (filterModel != null)
        {
            if (filterModel.MaxPriceFilter.HasValue)
            {
                FilterModel.MaxPriceFilter = filterModel.MaxPriceFilter.Value;
            }

            if (filterModel.MinPrice.HasValue)
            {
                FilterModel.MinPrice = filterModel.MinPrice.Value;
            }

            if (filterModel.SubcategoryId.HasValue)
            {
                FilterModel.SubcategoryId = filterModel.SubcategoryId.Value;
            }

            if (filterModel.CurrentPage != 0 && filterModel.CurrentPage != 1 && filterModel.CurrentPage != -1)
            {
                FilterModel.CurrentPage = 1;
            }
            else
            {
                FilterModel.CurrentPage += filterModel.CurrentPage;
            }
        }

        if (FilterModel.SubcategoryId != 0)
        {
            productsQuery = productsQuery.Where(e => e.SubcategoryId == FilterModel.SubcategoryId);
        }
        
        if(FilterModel.MaxPrice == null || FilterModel.MaxPrice == 0)
        {
			var maxPrice = await UnitOfWork.Products.Get()
	            .Where(p => p.RestaurantId == restaurantId)
	            .Select(p => (decimal?)p.Price)
	            .DefaultIfEmpty() 
	            .MaxAsync();
			FilterModel.MaxPrice = maxPrice.HasValue ? (int)Math.Ceiling(maxPrice.Value) : 0;
        }

        if (FilterModel.MaxPriceFilter == null || FilterModel.MaxPriceFilter == 0)
        {
            FilterModel.MaxPriceFilter = FilterModel.MaxPrice;
        }

        productsQuery = productsQuery.Where(e => e.Price >= FilterModel.MinPrice);
        productsQuery = productsQuery.Where(e => e.Price <= FilterModel.MaxPriceFilter);

        if (FilterModel.CurrentPage < 1)
        {
            FilterModel.CurrentPage = 1;
        }

        var numberOfProducts = productsQuery.Count();

        var elementsToSkip = (FilterModel.CurrentPage - 1) * FilterModel.ItemsOnPage;

        if (numberOfProducts != 0 && elementsToSkip > numberOfProducts)
        {
            FilterModel.CurrentPage--;
            elementsToSkip = (FilterModel.CurrentPage - 1) * FilterModel.ItemsOnPage;
        }

        var products = await productsQuery
            .OrderBy(p => p.SubcategoryId)
            .Skip(elementsToSkip)
            .Take(FilterModel.ItemsOnPage)
            .Select(t => Mapper.Map<Product, ViewProductModel>(t))
            .ToListAsync();

        if (products == null)
        {
            throw new NotFoundErrorException();
        }
        return products;
    }

    public async Task DeleteProduct(Guid productId)
    {
        var product = await UnitOfWork.Products.Get().SingleOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            throw new NotFoundErrorException("Product not found");
        }
        else
        {
            UnitOfWork.Products.Delete(product);
            await UnitOfWork.SaveChangesAsync();
        }
    }

    public FilterProductModel GetFiltersAndCurrentPage()
    {
        return FilterModel;
    }

    public async Task CheckForOwner(Guid restaurantId)
    {
        var restaurant = await UnitOfWork
            .Restaurants
            .Get()
            .SingleOrDefaultAsync(r => r.Id == restaurantId);

        if(restaurant == null)
        {
            throw new NotFoundErrorException();
        }

        if(restaurant.UserId != CurrentUser.Id && CurrentUser.Role != "Admin")
        {
            throw new AccessViolationException();
        }
    }

    public async Task<bool> IsMyRestaurant(Guid id)
    {
        return CurrentUser.Id == await UnitOfWork.Restaurants.Get().Where(r => r.Id == id).Select(r => r.UserId).SingleOrDefaultAsync();
    }
}
