using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Tables;
using Restaurant.BusinessLogic.Implementation.Tables.Models;
using Restaurant.Web.Code.Base;

namespace Restaurant.Web.Controllers
{
    public class TableController : BaseController
    {
        public readonly TableService Service;

        public TableController(ControllerDependencies dependencies, TableService service) : base(dependencies)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateTable(string restaurantName)
        {
            var model = new TableModel();
            model.RestaurantName = restaurantName;

            return View("CreateTable", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(TableModel model)
        {
            await Service.CreateTable(model);
            return RedirectToAction("Index", "Restaurant", new {name = model.RestaurantName});
        }
    }
}
