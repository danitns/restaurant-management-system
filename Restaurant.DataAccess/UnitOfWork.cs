using Restaurant.Common;
using Restaurant.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataAccess
{
    public class UnitOfWork
    {
        private readonly RestaurantContext Context;

        public UnitOfWork(RestaurantContext context)
        {
            Context = context;
        }

        private IRepository<City> cities;
        public IRepository<City> Cities => cities ?? (cities = new BaseRepository<City>(Context));


        private IRepository<Entities.Restaurant> restaurants;
        public IRepository<Entities.Restaurant> Restaurants => restaurants ?? (restaurants = new BaseRepository<Entities.Restaurant>(Context));

        private IRepository<RestaurantSchedule> restaurantSchedules;
        public IRepository<RestaurantSchedule> RestaurantSchedules => restaurantSchedules ?? (restaurantSchedules = new BaseRepository<RestaurantSchedule>(Context));


        private IRepository<Category> categories;
        public IRepository<Category> Categories => categories ?? (categories = new BaseRepository<Category>(Context));


        private IRepository<Product> products;
        public IRepository<Product> Products => products ?? (products = new BaseRepository<Product>(Context));


        private IRepository<Reservation> reservations;
        public IRepository<Reservation> Reservations => reservations ?? (reservations = new BaseRepository<Reservation>(Context));


        private IRepository<Role> roles;
        public IRepository<Role> Roles => roles ?? (roles = new BaseRepository<Role>(Context));


        private IRepository<Subcategory> subcategories;
        public IRepository<Subcategory> Subcategories => subcategories ?? (subcategories = new BaseRepository<Subcategory>(Context));


        private IRepository<Table> tables;
        public IRepository<Table> Tables => tables ?? (tables = new BaseRepository<Table>(Context));


        private IRepository<User> users;
        public IRepository<User> Users => users ?? (users = new BaseRepository<User>(Context));

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
