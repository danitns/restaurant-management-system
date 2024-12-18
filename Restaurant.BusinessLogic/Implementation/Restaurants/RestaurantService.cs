using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.Common.Exceptions;
using Restaurant.Common.Extensions;
using Restaurant.Entities;
using Restaurant.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Restaurants
{
	public class RestaurantService : BaseService
	{
		static private FilterRestaurantModel FilterModel;

		private readonly CreateRestaurantValidator CreateRestaurantValidator;
		public RestaurantService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
		{
			CreateRestaurantValidator = new CreateRestaurantValidator();
			if(FilterModel == null)
				FilterModel = new FilterRestaurantModel();
		}

		public async Task CreateRestaurant(CreateRestaurantModel model)
		{
			CreateRestaurantValidator.Validate(model).ThenThrow(model);

			var restaurant = Mapper.Map<CreateRestaurantModel, Entities.Restaurant>(model);

			restaurant.UserId = CurrentUser.Id;

			foreach(var schedule in restaurant.RestaurantSchedules)
			{
				schedule.RestaurantId = restaurant.Id;
			}

            if (model.Picture != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.Picture.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    restaurant.Picture = fileBytes;
                }
            }

            UnitOfWork.Restaurants.Insert(restaurant);
			await UnitOfWork.SaveChangesAsync();
		}

        public async Task<ViewRestaurantModel> GetDetails(Guid id)
        {
			var restaurant = await UnitOfWork.Restaurants
				.Get()
                .Include(r => r.Tables)
                .Include(r => r.RestaurantSchedules)
				.SingleOrDefaultAsync(r => r.Id == id);

			if(restaurant == null)
			{
				throw new NotFoundErrorException();
			}

			restaurant.RestaurantSchedules = restaurant.RestaurantSchedules.OrderBy(r => r.DayOfWeek).ToList();

			var mappedRestaurant = Mapper.Map<Entities.Restaurant, ViewRestaurantModel>(restaurant);

			mappedRestaurant.IsMyRestaurant = CurrentUser.Id == restaurant.UserId;

			return mappedRestaurant;
        }

        public FilterRestaurantModel GetFiltersAndCurrentPage()
        {
			return FilterModel;
        }

		public async Task<List<ViewRestaurantModel>> GetRestaurantsByManager()
		{
			var restaurants = await UnitOfWork.Restaurants
				.Get()
				.Where(r => r.UserId == CurrentUser.Id)
				.ProjectTo<ViewRestaurantModel>(Mapper.ConfigurationProvider)
				.ToListAsync();
			return restaurants;
		}

        public async Task<List<ViewRestaurantModel>> GetRestaurants(FilterRestaurantModel? filterModel)
		{
			var restaurantsQuery = UnitOfWork.Restaurants
				.Get();

			if(filterModel != null)
			{
				if(filterModel.CityId.HasValue)
				{
					FilterModel.CityId = filterModel.CityId.Value;
				}

				if(filterModel.TypeId.HasValue)
				{
					FilterModel.TypeId = filterModel.TypeId.Value;
				}

				if(filterModel.IsMine.HasValue)
				{
					FilterModel.IsMine = filterModel.IsMine;
				}

				if(filterModel.CurrentPage != 0 && filterModel.CurrentPage != 1 && filterModel.CurrentPage != -1)
				{
					FilterModel.CurrentPage = 1;
				}
				else
				{
					FilterModel.CurrentPage += filterModel.CurrentPage;
				}
			}

			if(FilterModel.CityId != 0)
			{
				restaurantsQuery = restaurantsQuery.Where(e => e.CityId == FilterModel.CityId);
			}

            if (FilterModel.TypeId != 0)
            {
                restaurantsQuery = restaurantsQuery.Where(e => e.RestaurantTypeId == FilterModel.TypeId);
            }

			if (FilterModel.IsMine == true)
			{
                restaurantsQuery = restaurantsQuery.Where(e => e.UserId == CurrentUser.Id);
            }

			if(FilterModel.CurrentPage < 1)
			{
				FilterModel.CurrentPage = 1;
			}

			var numberOfRestaurants = restaurantsQuery.Count();

			var elementsToSkip = (FilterModel.CurrentPage - 1) * FilterModel.ItemsOnPage;

			if(numberOfRestaurants != 0 && elementsToSkip > numberOfRestaurants)
			{
				FilterModel.CurrentPage--;
                elementsToSkip = (FilterModel.CurrentPage - 1) * FilterModel.ItemsOnPage;
            }

			var restaurants = await restaurantsQuery
				.Skip(elementsToSkip)
				.Take(FilterModel.ItemsOnPage)
				.Select(t => Mapper.Map<Entities.Restaurant, ViewRestaurantModel>(t))
				.ToListAsync();

			if(restaurants == null)
			{
				throw new NotFoundErrorException();
			}

            return restaurants;
		}

	}
}
