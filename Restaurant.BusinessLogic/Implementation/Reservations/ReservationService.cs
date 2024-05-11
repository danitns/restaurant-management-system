using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Reservations.Validations;
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

		public async Task CreateReservation(CreateReservationModel model)
		{
			CreateReservationValidator.Validate(model).ThenThrow(model);
			var reservation = Mapper.Map<CreateReservationModel, Reservation>(model);

			UnitOfWork.Reservations.Insert(reservation);
			await UnitOfWork.SaveChangesAsync();
		}

		public async Task<List<string>> GetAvailableHours(DateOnly date, int numberOfGuests)
		{
			var tables = await UnitOfWork.Tables.Get().OrderBy(t => t.Seats).Select(t => t.Id).ToListAsync();
			var reservations = await UnitOfWork.Reservations
				.Get()
				.Include(r => r.Table)
				.Where(r => DateOnly.FromDateTime(r.Date).Equals(date)
					&& r.Table != null
					&& r.Table.Seats >= numberOfGuests)
				.Select(r => new { TableId = r.TableId, Date = r.Date })
				.ToListAsync();

			var availabilityMatrix = InitializeAvailabilityMatrix(tables.Count());

			foreach (var reservation in reservations)
			{
				var startIndex = AllReservationHours.FindIndex(a => a == TimeOnly.FromDateTime(reservation.Date));
				var endIndex = startIndex + 2;
				for (int i = startIndex; i < endIndex && i < AllReservationHours.Count(); i++)
				{
					availabilityMatrix[reservations.IndexOf(reservation)][i] = 1;
				}
			}

			var availableIntervals = FindAvailableIntervals(availabilityMatrix);

			return availableIntervals;
		}

		private int[][] InitializeAvailabilityMatrix(int numberOfTables)
		{
			int[][] availabilityMatrix = new int[numberOfTables][];
			for (int i = 0; i < numberOfTables; i++)
			{
				availabilityMatrix[i] = new int[AllReservationHours.Count()];
			}
			return availabilityMatrix;
		}

		private List<string> FindAvailableIntervals(int[][] availabilityMatrix)
		{
			var availableIntervals = new List<string>();

			foreach (var row in availabilityMatrix)
			{
				int start = -1;
				for (int i = 0; i < row.Length; i++)
				{
					if (row[i] == 0)
					{
						if (start == -1)
						{
							start = i;
						}
					}
					else
					{
						if (start != -1 && i - start >= 2)
						{
							availableIntervals.Add($"{AllReservationHours[start]} - {AllReservationHours[i - 1]}");
						}
						start = -1;
					}
				}
				
				if (start != -1 && row[row.Length - 1] == 0)
				{
					availableIntervals.Add($"{AllReservationHours[start]} - {AllReservationHours[row.Length - 1]}");
				}
			}

			return availableIntervals;
		}
	}
}
