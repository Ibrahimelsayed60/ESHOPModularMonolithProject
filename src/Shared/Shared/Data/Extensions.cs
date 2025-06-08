using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            return appBuilder;
        }

        private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider applicationServices) where TContext : DbContext
        {
            using var scope = applicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            await context.Database.MigrateAsync();
        }
    }
}
