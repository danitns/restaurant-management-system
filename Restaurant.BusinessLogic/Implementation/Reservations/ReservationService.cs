using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Reservations.Validations;
using Restaurant.BusinessLogic.Implementation.Restaurants.Models;
using Restaurant.Common.Exceptions;
using Restaurant.Common.Extensions;
using Restaurant.Entities;
using Restaurant.Entities.Enums;
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
				.Include(r => r.Review)
				.Where(r => r.UserId == CurrentUser.Id)
				.ToListAsync();
			var mappedReservations = Mapper.Map<List<Reservation>, List<ViewReservationModel>>(reservations);
			return mappedReservations;
		}

		public async Task<List<ViewReservationModel>> GetAllReservationsByRestaurantId(Guid restaurantId)
		{
			var reservations = await UnitOfWork.Reservations
				.Get()
				.Include (r => r.Table)
				.ThenInclude(t => t.Restaurant)
				.Where(x => x.Table.RestaurantId == restaurantId)
				.ProjectTo<ViewReservationModel>(Mapper.ConfigurationProvider)
				.ToListAsync();
			return reservations;
		}

		public async Task<ViewReservationModel> GetReservationById(Guid id)
		{
			var reservation = await UnitOfWork.Reservations
				.Get()
				.Include(r => r.Review)
				.ThenInclude(rr => rr.ProductReviews)
				.FirstOrDefaultAsync(r => r.Id == id);
			var mappedReservation = Mapper.Map<ViewReservationModel>(reservation);
			return mappedReservation;
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

        public async Task CompleteOrder(CreateOrderModel orderItems)
        {
			var review = new Review
			{
				Id = orderItems.ReservationId,

			};
			var productReviewList = new List<ProductReview>();

            var productIds = orderItems.Items.Select(item => item.ProductId).ToList();

			var products = UnitOfWork.Products.Get()
				.Where(product => productIds.Contains(product.Id))
				.ToList();

			decimal totalPrice = 0;

			foreach (var orderItem in orderItems.Items)
			{
				var product = products.FirstOrDefault(p => p.Id == orderItem.ProductId);

				if (product != null)
				{
					totalPrice += product.Price * orderItem.Quantity;

					productReviewList.Add(new ProductReview
					{
						Id = Guid.NewGuid(),
						ProductId = orderItem.ProductId,
						Quantity = orderItem.Quantity,
					});
				}
			}

			review.ProductReviews = productReviewList;

			var reservation = await UnitOfWork.Reservations
				.Get()
				.SingleOrDefaultAsync(r => r.Id == orderItems.ReservationId);

			if(reservation == null)
			{
				throw new NotFoundErrorException();
			}

			reservation.Status = (short)ReservationStatusTypes.Completed;
			reservation.TotalAmount = ((int)totalPrice);


			var user = await UnitOfWork.Users.Get().FirstOrDefaultAsync(u => u.Id == reservation.UserId);
			user.Points += (int)totalPrice;

			UnitOfWork.Reviews.Insert(review);
			UnitOfWork.Reservations.Update(reservation);
			UnitOfWork.Users.Update(user);
			await UnitOfWork.SaveChangesAsync();
        }

		public async Task<ReviewReservationModel> GetReservationAndProductsToReview(Guid id)
		{
			var review = await UnitOfWork.Reviews
				.Get()
				.Include(r => r.ProductReviews)
					.ThenInclude(pr => pr.Product)
				.Include(r => r.IdNavigation)
					.ThenInclude(rr => rr.Table)
					.ThenInclude(t => t.Restaurant)
				.SingleOrDefaultAsync(r => r.Id == id);

			var productReviewsMapped = new List<ReviewProductModel>();

			var mappedReview = Mapper.Map<ReviewReservationModel>(review);

			foreach(var productRev in review.ProductReviews)
			{
				productReviewsMapped.Add(Mapper.Map<ReviewProductModel>(productRev));
			}

			mappedReview.ProductReviews = productReviewsMapped;

			return mappedReview;
		}

		public async Task ReviewReservation(ReviewReservationModel model)
		{
			var review = await UnitOfWork.Reviews
				.Get()
				.Include(r => r.ProductReviews)
				.Include(r => r.IdNavigation)
					.ThenInclude(res => res.Table)
					.ThenInclude(t => t.Restaurant)
				.SingleOrDefaultAsync(r => r.Id == model.ReservationId);

            var reservation = await UnitOfWork.Reservations
				.Get()
				.SingleOrDefaultAsync(r => r.Id == review.Id);

			if(review == null || reservation == null)
			{
				throw new NotFoundErrorException();
			}

            review.Text = model.Text;
			review.Rating = model.Rating;
			
			foreach(var productRev in review.ProductReviews)
			{
				var productReview = model.ProductReviews.FirstOrDefault(pr => pr.Id == productRev.Id);
				productRev.Text = productReview.Text;
				productRev.Rating = productReview.Rating;

				if (productReview.Picture != null)
				{
					using (var ms = new MemoryStream())
					{
						productReview.Picture.CopyTo(ms);
						var fileBytes = ms.ToArray();
						productRev.ImageContent = fileBytes;
					}
				}
			}

			reservation.Status = (int)ReservationStatusTypes.Reviewed;
			var bonusPoints = (int)(reservation.TotalAmount * 0.1);

			reservation.TotalAmount += bonusPoints;
			var user = await UnitOfWork.Users
				.Get()
				.SingleOrDefaultAsync(u => u.Id == reservation.UserId);

			if(user == null)
			{
				throw new NotFoundErrorException();
			}

			user.Points += bonusPoints;

			UnitOfWork.Reviews.Update(review);
			UnitOfWork.Reservations.Update(reservation);
			await UnitOfWork.SaveChangesAsync();
		}
	}
}
