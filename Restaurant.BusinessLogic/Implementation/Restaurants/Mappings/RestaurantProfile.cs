using AutoMapper;
using Restaurant.BusinessLogic.Implementation.Restaurants;	
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
			CreateMap<CreateRestaurantModel, Entities.Restaurant>()
				.ForMember(d => d.Id, d => d.MapFrom(s => Guid.NewGuid()))
				.ForMember(d => d.UserId, d => d.Ignore())
				.ForMember(d => d.Picture, d => d.Ignore());

			CreateMap<Entities.Restaurant, ViewRestaurantModel>()
				.ForMember(d => d.CityName, d => d.MapFrom(s => Enum.GetName(typeof(Cities), s.CityId)));
		}
	}
}
