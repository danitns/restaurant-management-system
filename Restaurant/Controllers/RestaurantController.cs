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

		public IActionResult Index()
		{
			var restaurants = Service.GetRestaurants();
			return View(restaurants);
		}

		[HttpGet]
		public IActionResult Create()
		{
			var model = new CreateRestaurantModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateRestaurantModel model)
		{
			await Service.CreateRestaurant(model);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Details(string name)
		{
			var model = await Service.GetDetails(name);
			return View(model);
		}
	}
}
