using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> CreateTable(Guid id)
        {
            var model = new TableModel();
            model.RestaurantId = id;

            await Service.CheckForOwner(id);

            return View("CreateTable", model);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateTable(TableModel model)
        {
            await Service.CreateTable(model);
            return RedirectToAction("Index", "Restaurant", new { Id = model.RestaurantId});
        }
    }
}
