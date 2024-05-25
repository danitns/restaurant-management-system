using Microsoft.AspNetCore.Mvc;
using Restaurant.Web.Code.Base;
using Restaurant.Web.Code.Utils;
using Restaurant.BusinessLogic.Implementation.Products;
using Restaurant.BusinessLogic.Implementation.Products.Models;

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

        return View("Index", model);
    }

    [HttpGet]
    public IActionResult CreateProduct(Guid Id)
    {
        var model = new CreateProductModel();
        model.RestaurantId = Id;

        return View("CreateProduct", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductModel model)
    {
        await Service.CreateProduct(model);

        return RedirectToAction("Index", "Product", new { Id = model.RestaurantId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, Guid restaurantId)
    {
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
