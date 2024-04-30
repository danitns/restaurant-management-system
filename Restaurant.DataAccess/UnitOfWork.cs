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

        private IRepository<Bill> bills;
        public IRepository<Bill> Bills => bills ?? (bills = new BaseRepository<Bill>(Context));


        private IRepository<BillProduct> billProducts;
        public IRepository<BillProduct> BillsProducts => billProducts ?? (billProducts = new BaseRepository<BillProduct>(Context));


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
