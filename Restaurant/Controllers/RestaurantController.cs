using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Restaurants;
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
        public IActionResult GetFiltersAndCurrentPage()
        {
            var filtersAndPagination = Service.GetFiltersAndCurrentPage();
            return Ok(filtersAndPagination);
        }
    }
}
