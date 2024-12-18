using AutoMapper;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Reservations.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
			CreateMap<CreateReservationModel, Reservation>()
				.ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
				.ForMember(a => a.Date, a => a.MapFrom(s => CombineDateTime(s.Date, s.Time)))
				.ForMember(a => a.UserId, a => a.Ignore());

			CreateMap<Reservation, ViewReservationModel>()
				.ForMember(a => a.RestaurantName, a => a.MapFrom(s => s.Table.Restaurant.Name))
				.ForMember(a => a.Address, a => a.MapFrom(s => s.Table.Restaurant.Address))
				.ForMember(a => a.RestaurantPicture, a => a.MapFrom(s => s.Table.Restaurant.Picture))
				.ForMember(a => a.RestaurantId, a => a.MapFrom(s => s.Table.RestaurantId))
				.ForMember(a => a.TotalPrice, a => a.MapFrom(s => s.TotalAmount))
				.ForMember(a => a.Rating, a => a.MapFrom(s => s.Review.Rating))
				.ForMember(a => a.TableName, a => a.MapFrom(s => s.Table.Name));

			CreateMap<Review, ReviewReservationModel>()
				.ForMember(a => a.RestaurantName, a => a.MapFrom(s => s.IdNavigation.Table.Restaurant.Name))
				.ForMember(a => a.ReservationId, a => a.MapFrom(s => s.Id))
				.ForMember(a => a.Text, a => a.Ignore())
				.ForMember(a => a.Rating, a => a.Ignore())
				.ForMember(a => a.ProductReviews, a => a.Ignore());

			CreateMap<ProductReview, ReviewProductModel>()
				.ForMember(a => a.ProductId, a => a.MapFrom(s => s.ProductId))
                .ForMember(a => a.ProductName, a => a.MapFrom(s => s.Product.Name))
                .ForMember(a => a.Price, a => a.MapFrom(s => s.Product.Price))
                .ForMember(a => a.Picture, a => a.Ignore())
				.ForMember(a => a.Text, a => a.Ignore())
				.ForMember(a => a.Rating, a => a.Ignore())
				.ForMember(a => a.ProductPicture, a => a.MapFrom(s => s.Product.Picture));
		}

		private DateTime CombineDateTime(DateOnly date, string timeAsString)
		{
			var modelTimeParseResult = TimeOnly.TryParse(timeAsString, out var time);

			if (modelTimeParseResult == false)
			{
				throw new FormatException();
			}
			return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
		}
	}
}
