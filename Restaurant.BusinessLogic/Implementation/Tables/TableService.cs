using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Tables.Models;
using Restaurant.BusinessLogic.Implementation.Tables.Validations;
using Restaurant.Common.Exceptions;
using Restaurant.Common.Extensions;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BusinessLogic.Implementation.Tables
{
    public class TableService : BaseService
    {
        private readonly TableValidator tableValidator;

        public TableService(ServiceDependencies serviceDependencies ) : base( serviceDependencies ) {
            tableValidator = new TableValidator(UnitOfWork);
        }  

        public async Task CreateTable(TableModel model)
        {
            tableValidator.Validate(model).ThenThrow(model);
            var table = Mapper.Map<TableModel, Table>(model);

            table.RestaurantId = await GetRestaurantIdByName(model.RestaurantName);

            UnitOfWork.Tables.Insert(table);
            UnitOfWork.SaveChanges();
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
