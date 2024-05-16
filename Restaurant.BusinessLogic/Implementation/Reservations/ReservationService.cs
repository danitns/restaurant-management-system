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
		private static readonly List<TimeOnly> AllReservationHours = new List<TimeOnly>
		{
			new TimeOnly(10, 0),  // 10:00
			new TimeOnly(11, 0),  // 11:00
			new TimeOnly(12, 0),  // 12:00
			new TimeOnly(13, 0),  // 13:00
			new TimeOnly(14, 0),  // 14:00
			new TimeOnly(15, 0),  // 15:00
			new TimeOnly(16, 0),  // 16:00
			new TimeOnly(17, 0),  // 17:00
			new TimeOnly(18, 0),  // 18:00
			new TimeOnly(19, 0),  // 19:00
			new TimeOnly(20, 0),  // 20:00
			new TimeOnly(21, 0),  // 21:00
			new TimeOnly(22, 0)   // 22:00
		};


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

			var restaurantId = await GetRestaurantIdByName(model.RestaurantName);

			var tables = await UnitOfWork.Tables
				.Get()
				.Include(t => t.Reservations)
				.Where(t => t.Seats >= model.NumberOfGuests && t.RestaurantId == restaurantId)
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

		public async Task<List<string>> GetAvailableHours(DateOnly date, int numberOfGuests, string restaurantName)
		{
			var restaurantId = await GetRestaurantIdByName(restaurantName);

			var tables = await UnitOfWork.Tables
				.Get()
				.Include(t => t.Reservations)
				.Where(t => t.Seats >= numberOfGuests && t.RestaurantId == restaurantId)
				.OrderBy(t => t.Seats)
				.ToListAsync();

			var freeIntervals = new List<string>();

			foreach (var time in AllReservationHours)
			{
				var isFree = tables
					.Where(t => !t.Reservations.Any(r => DateOnly.FromDateTime(r.Date) == date && (TimeOnly.FromDateTime(r.Date) == time
						|| TimeOnly.FromDateTime(r.Date) == time.AddHours(1)
						|| TimeOnly.FromDateTime(r.Date) == time.AddHours(-1))))
					.Any();

				if (isFree == true)
				{
					freeIntervals.Add(time.ToString());
				}
			}
			return freeIntervals;
		}

		private async Task<Guid> GetRestaurantIdByName(string name)
		{
			var restaurant = await UnitOfWork.Restaurants.Get().SingleOrDefaultAsync(r => r.Name == name);

			if (restaurant == null)
			{
				throw new NotFoundErrorException();
			}

			return restaurant.Id;
		}
	}
}
