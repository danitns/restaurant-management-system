using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Reservations;
using Restaurant.BusinessLogic.Implementation.Restaurants.Models;
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

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RestaurantReservations(Guid id)
        {
            var reservations = await Service.GetAllReservationsByRestaurantId(id);
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

        [HttpPost]
        public async Task<IActionResult> CompleteOrder([FromBody]CreateOrderModel orderItems)
        {
            if (orderItems == null || orderItems.Items == null || orderItems.Items.Count == 0)
            {
                return BadRequest(new { message = "Invalid order data." });
            }

            try
            {
                await Service.CompleteOrder(orderItems);
                return Ok(new { message = "Order completed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReviewReservation(Guid id)
        {
            var reservationWithProducts = await Service.GetReservationAndProductsToReview(id);
            return View(reservationWithProducts);
        }

		[HttpPost]
		public async Task<IActionResult> ReviewReservation(ReviewReservationModel model)
		{
			await Service.ReviewReservation(model);
			return RedirectToAction("Index");
		}
	}
}
