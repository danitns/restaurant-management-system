using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Code.Base;
using Restaurant.Web.Code.Utils;
using Restaurant.BusinessLogic.Implementation.Products;
using Restaurant.BusinessLogic.Implementation.Products.Models;
using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Web.Controllers;

public class ProductController : BaseController
{
    public readonly ProductService Service;
    public ProductController(ControllerDependencies dependencies, ProductService service) : base(dependencies)
    {
        Service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid Id, FilterProductModel? filterModel = null)
    {
        var model = await Service.GetProducts(Id, filterModel);
        ViewBag.RestaurantId = Id;
        ViewBag.IsMyRestaurant = await Service.IsMyRestaurant(Id);

        return View("Index", model);
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateProduct(Guid Id)
    {
        var model = new CreateProductModel();
        await Service.CheckForOwner(Id);
        model.RestaurantId = Id;

        return View("CreateProduct", model);
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductModel model)
    {
        await Service.CreateProduct(model);

        return RedirectToAction("Index", "Product", new { Id = model.RestaurantId });
    }

    [Authorize(Roles = "Manager,Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, Guid restaurantId)
    {
        await Service.CheckForOwner(restaurantId);
        await Service.DeleteProduct(id);

        return RedirectToAction("Index", new {Id = restaurantId});
    }

    [HttpGet]
    public IActionResult GetFiltersAndCurrentPage()
    {
        var filtersAndPagination = Service.GetFiltersAndCurrentPage();
        return Ok(filtersAndPagination);
    }
}
