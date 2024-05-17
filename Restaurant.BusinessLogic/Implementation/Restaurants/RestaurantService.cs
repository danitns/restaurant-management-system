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
		private readonly CreateRestaurantValidator CreateRestaurantValidator;
		public RestaurantService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
		{
			CreateRestaurantValidator = new CreateRestaurantValidator();
		}

		public async Task CreateRestaurant(CreateRestaurantModel model)
		{
			CreateRestaurantValidator.Validate(model).ThenThrow(model);

			var restaurant = Mapper.Map<CreateRestaurantModel, Entities.Restaurant>(model);

			restaurant.UserId = CurrentUser.Id;

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
			var restaurant = await UnitOfWork.Restaurants.Get().SingleOrDefaultAsync(r => r.Id == id);

			if(restaurant == null)
			{
				throw new NotFoundErrorException();
			}

			return Mapper.Map<Entities.Restaurant, ViewRestaurantModel>(restaurant);
        }

        public List<ViewRestaurantModel> GetRestaurants()
		{
			var restaurants = UnitOfWork.Restaurants.Get().ToList().Select(r => Mapper.Map<Entities.Restaurant, ViewRestaurantModel>(r)).ToList();
			return restaurants;
		}

	}
}
