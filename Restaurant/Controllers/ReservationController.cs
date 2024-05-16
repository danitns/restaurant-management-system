using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Reservations;
using Restaurant.Web.Code.Base;

namespace Restaurant.Web.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly ReservationService Service;

        public ReservationController(ControllerDependencies dependencies, ReservationService service) : base(dependencies)
        {
            Service = service;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await Service.GetReservations();
            return View(reservations);
        }

        [HttpGet]
        public IActionResult Create(string restaurantName)
        {
            var model = new CreateReservationModel();
            model.RestaurantName = restaurantName;
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationModel model)
        {
            await Service.CreateReservation(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<List<string>> GetAvailableHours(DateOnly date, int numberOfGuests, string restaurantName)
        {
            var availableHours = await Service.GetAvailableHours(date, numberOfGuests, restaurantName);
            return availableHours;
        }
    }
}
