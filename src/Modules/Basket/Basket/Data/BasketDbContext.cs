using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Data
{
    public class BasketDbContext : DbContext
    {

        public BasketDbContext(DbContextOptions<BasketDbContext> options): base(options)
        {
            
        }

        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

        public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("basket");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

    }
}
