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
    public async Task<IActionResult> Index()
    {
        var model = await Service.GetProducts();

        return View("Index", model);
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        var model = new CreateProductModel();

        return View("CreateProduct", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductModel model)
    {
        await Service.CreateProduct(model);

        return RedirectToAction("Index", "Home");
    }
}
