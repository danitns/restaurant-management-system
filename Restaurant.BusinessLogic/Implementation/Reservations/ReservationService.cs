using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Reservations.Validations;
using Restaurant.Common.Exceptions;
using Restaurant.Common.Extensions;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations
{
	public class ReservationService : BaseService
	{
		private readonly CreateReservationValidator CreateReservationValidator;
		public ReservationService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
		{
			CreateReservationValidator = new CreateReservationValidator();
		}

		public async Task<List<ViewReservationModel>> GetReservations()
		{
			var reservations = await UnitOfWork.Reservations
				.Get()
				.Include(r => r.Table)
				.ThenInclude(t => t.Restaurant)
				.Where(r => r.UserId == CurrentUser.Id)
				.ToListAsync();
			var mappedReservations = Mapper.Map<List<Reservation>, List<ViewReservationModel>>(reservations);
			return mappedReservations;
		}

		public async Task CreateReservation(CreateReservationModel model)
		{
			CreateReservationValidator.Validate(model).ThenThrow(model);
			var reservation = Mapper.Map<CreateReservationModel, Reservation>(model);

			var modelTimeParseResult = TimeOnly.TryParse(model.Time, out var parsedTime);

			if (modelTimeParseResult == false)
			{
				throw new FormatException();
			}

			var tables = await UnitOfWork.Tables
				.Get()
				.Include(t => t.Reservations)
				.Where(t => t.Seats >= model.NumberOfGuests && t.RestaurantId == model.RestaurantId)
				.OrderBy(t => t.Seats)
				.ToListAsync();

			var availableTable = tables.FirstOrDefault(t =>
				!t.Reservations.Any(r => TimeOnly.FromDateTime(r.Date) == parsedTime ||
										   TimeOnly.FromDateTime(r.Date) == parsedTime.AddHours(1) ||
										   TimeOnly.FromDateTime(r.Date) == parsedTime.AddHours(-1)));

			if (availableTable == null)
			{
				throw new NotFoundErrorException();
			}

			reservation.TableId = availableTable.Id;
			if (CurrentUser.IsAuthenticated)
			{
				reservation.UserId = CurrentUser.Id;
			}

			UnitOfWork.Reservations.Insert(reservation);
			await UnitOfWork.SaveChangesAsync();
		}

		public async Task<List<string>> GetAvailableHours(DateOnly date, int numberOfGuests, Guid restaurantId)
		{

			var tables = await UnitOfWork.Tables
				.Get()
				.Include(t => t.Reservations)
				.Where(t => t.Seats >= numberOfGuests && t.RestaurantId == restaurantId)
				.OrderBy(t => t.Seats)
				.ToListAsync();

			var allReservationHours = await GetReservationHours(restaurantId);

            var freeIntervals = new List<string>();

			foreach (var time in allReservationHours)
			{
				var isFree = tables
					.Where(t => !t.Reservations.Any(r => DateOnly.FromDateTime(r.Date) == date && (TimeOnly.FromDateTime(r.Date) == time
						|| TimeOnly.FromDateTime(r.Date) == time.AddHours(1)
						|| TimeOnly.FromDateTime(r.Date) == time.AddHours(-1))))
					.Any();

				if (isFree == true && date == DateOnly.FromDateTime(DateTime.Now) && time < TimeOnly.FromDateTime(DateTime.Now))
					isFree = false;

				if (isFree == true)
				{
					freeIntervals.Add(time.ToString());
				}
			}
			return freeIntervals;
		}

		private async Task<List<TimeOnly>> GetReservationHours(Guid restaurantId)
		{
			var dayOfWeek = (int)DateTime.Now.DayOfWeek;

			var restaurantSchedule = await UnitOfWork.RestaurantSchedules
				.Get()
				.Where(r => r.RestaurantId == restaurantId && r.DayOfWeek == dayOfWeek)
				.SingleOrDefaultAsync();

            if (restaurantSchedule == null)
            {
                throw new NotFoundErrorException();
            }
            var allReservationHours = new List<TimeOnly>();

            for (TimeOnly time = restaurantSchedule.OpeningTime; time <= restaurantSchedule.ClosingTime.AddHours(-2); time = time.AddHours(1))
            {
                allReservationHours.Add(time);
            }

			return allReservationHours;

        }
	}
}
