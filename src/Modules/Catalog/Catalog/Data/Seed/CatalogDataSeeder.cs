using Microsoft.EntityFrameworkCore;
using Shared.Data.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Data.Seed
{
    public class CatalogDataSeeder : IDataSeeder
    {
        private readonly CatalogDbContext _dbContext;

        public CatalogDataSeeder(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAllAsync()
        {
            if(!await _dbContext.Products.AnyAsync())
            {
                await _dbContext.Products.AddRangeAsync(IntitialData.Products);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
