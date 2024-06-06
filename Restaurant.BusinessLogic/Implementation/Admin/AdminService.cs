using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Restaurants;
using Restaurant.BusinessLogic.Implementation.Users;
using Restaurant.Common.Exceptions;
using Restaurant.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Admin
{
	public class AdminService : BaseService
	{
		public AdminService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
		{
		}


		public async Task<AdminDashboardModel> GetAdminDashboardData()
		{
			AdminDashboardModel model = new AdminDashboardModel();
			model.PendingManagers = await GetPendingManagers();
			model.RestaurantTop = await GetRestaurantTop();
			return model;
		}

		private async Task<List<RestaurantAndReservations>> GetRestaurantTop()
		{
			var restaurants = await UnitOfWork.Restaurants
				.Get()
				.Include(r => r.RestaurantSchedules)
				.OrderBy(r => r.RestaurantSchedules.Count())
				.Select(r => Mapper.Map<RestaurantAndReservations>(r))
				.ToListAsync();

			return restaurants;
		}

		private async Task<List<DetailsUserModel>> GetPendingManagers()
		{
			var managers = await UnitOfWork.Users.Get().Where(u => u.RoleId == (int)RoleTypes.PendingManager).ToListAsync();
			var mappedManagers = Mapper.Map<List<DetailsUserModel>>(managers);
			return mappedManagers;
		}

		public async Task UpdateUserRole(Guid userId, bool isApproved)
		{
			var user = await UnitOfWork.Users.Get().SingleOrDefaultAsync(u => u.Id == userId);
			if (user == null)
			{
				throw new NotFoundErrorException();
			}
			user.RoleId = isApproved ? (int)RoleTypes.Manager : (int)RoleTypes.User;
			UnitOfWork.Users.Update(user);
			await UnitOfWork.SaveChangesAsync();
		}
	}
}
