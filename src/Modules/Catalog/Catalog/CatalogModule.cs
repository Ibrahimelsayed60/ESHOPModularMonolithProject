using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace Catalog
{
    public static class CatalogModule
    {

        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Services to the container

            var connectionstring = configuration.GetConnectionString("Database");

            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseNpgsql(connectionstring);
            });

            return services;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline

            app.UseMigration<CatalogDbContext>();

            return app;
        }


    }
}
