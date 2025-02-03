using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Restaurants;
using Restaurant.BusinessLogic.Implementation.Restaurants.Models;
using Restaurant.Web.Code.Base;

namespace Restaurant.Web.Controllers
{
	public class RestaurantController : BaseController
	{
		private readonly RestaurantService Service;
		public RestaurantController(ControllerDependencies dependencies, RestaurantService service) : base(dependencies)
		{
			Service = service;
		}

		public async Task<IActionResult> Index(FilterRestaurantModel filterModel = null)
		{
			var restaurants = await Service.GetRestaurants(filterModel);
			return View(restaurants);
		}

		[Authorize(Roles = "Admin,Manager")]
		public async Task<IActionResult> OwnedRestaurants()
		{
			var restaurants = await Service.GetRestaurantsByManager();
			return View(restaurants);
		}

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
		public IActionResult Create()
		{
			var model = new CreateRestaurantModel();
			for(int i = 0; i < 7; i++)
			{
				model.Schedules[i] = new RestaurantScheduleModel();
			}
			return View(model);
		}

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
		public async Task<IActionResult> Create(CreateRestaurantModel model)
		{
			await Service.CreateRestaurant(model);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Details(Guid Id)
		{
			var model = await Service.GetDetails(Id);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> CreateBenefit(Guid Id)
		{
			var model = new CreateBenefitModel();
			model.RestaurantId = Id;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateBenefit(CreateBenefitModel model)
		{
			await Service.CreateBenefit(model);
			return RedirectToAction("Details", model.RestaurantId);
		}

        [HttpGet]
        public IActionResult GetFiltersAndCurrentPage()
        {
            var filtersAndPagination = Service.GetFiltersAndCurrentPage();
            return Ok(filtersAndPagination);
        }

		[HttpPost]
		public async Task<IActionResult> AssignBenefit(Guid? id = null, Guid? reservationId = null)
		{
			await Service.AssignBenefit(id, reservationId);
			return Ok();
		}
    }
}
