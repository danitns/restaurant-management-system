using AutoMapper;
using Restaurant.BusinessLogic.Implementation.Restaurants;
using Restaurant.Entities;
using Restaurant.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants.Mappings
{
	public class RestaurantProfile : Profile
	{
		public RestaurantProfile() 
		{
			CreateMap<RestaurantScheduleModel, RestaurantSchedule>()
				.ForMember(d => d.Id, d => d.MapFrom(s => Guid.NewGuid()))
				.ForMember(d => d.RestaurantId, d => d.Ignore());

            CreateMap<RestaurantSchedule, RestaurantScheduleModel>();

            CreateMap<CreateRestaurantModel, Entities.Restaurant>()
				.ForMember(d => d.Id, d => d.MapFrom(s => Guid.NewGuid()))
				.ForMember(d => d.RestaurantSchedules, d => d.MapFrom(s => s.Schedules))
				.ForMember(d => d.UserId, d => d.Ignore())
				.ForMember(d => d.Picture, d => d.Ignore());

			CreateMap<Entities.Restaurant, ViewRestaurantModel>()
				.ForMember(d => d.CityName, d => d.MapFrom(s => Enum.GetName(typeof(Cities), s.CityId)))
                .ForMember(d => d.TypeName, d => d.MapFrom(s => Enum.GetName(typeof(RestaurantTypeEnum), s.RestaurantTypeId)))
				.ForMember(d => d.Schedules, d => d.MapFrom(s => s.RestaurantSchedules))
				.ForMember(d => d.Tables, d => d.MapFrom(s => s.Tables))
				.ForMember(d => d.IsMyRestaurant, d => d.Ignore());

			CreateMap<Entities.Restaurant, RestaurantAndReservations>()
				.ForMember(d => d.NumberOfReservations, d => d.MapFrom(s => s.RestaurantSchedules.Count()));
		}
	}
}
