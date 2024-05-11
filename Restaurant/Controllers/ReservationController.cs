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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateReservationModel();
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationModel model)
        {
            await Service.CreateReservation(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<List<string>> GetAvailableHours(DateOnly date, int numberOfGuests)
        {
            var availableHours = await Service.GetAvailableHours(date, numberOfGuests);
            return availableHours;
        }
    }
}
