using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data
{
    public static class Extensions
    {

        public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder appBuilder) where TContext:DbContext
        {
            MigrateDatabaseAsync<TContext>(appBuilder.ApplicationServices).GetAwaiter().GetResult();

            SeedDataAsync(appBuilder.ApplicationServices).GetAwaiter().GetResult();

            return appBuilder;
        }

        

        private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider applicationServices) where TContext : DbContext
        {
            using var scope = applicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            await context.Database.MigrateAsync();
        }

        private static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

            foreach (var seeder in seeders)
            {
                await seeder.SeedAllAsync();
            }
        }
    }
}
