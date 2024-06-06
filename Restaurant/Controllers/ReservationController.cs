using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var reservations = await Service.GetReservations();
            return View(reservations);
        }

        [HttpGet]
        public IActionResult Create(Guid Id)
        {
            var model = new CreateReservationModel();
            model.RestaurantId = Id;
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationModel model)
        {
            await Service.CreateReservation(model);
            return RedirectToAction("Index", "Restaurant");
        }

        [HttpGet]
        public async Task<List<string>> GetAvailableHours(DateOnly date, int numberOfGuests, Guid restaurantId)
        {
            var availableHours = await Service.GetAvailableHours(date, numberOfGuests, restaurantId);
            return availableHours;
        }
    }
}
