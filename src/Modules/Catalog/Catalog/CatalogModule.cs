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
using Shared.Data.Seed;
using Catalog.Data.Seed;
using Shared.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using FluentValidation;
using Shared.Behaviors;

namespace Catalog
{
    public static class CatalogModule
    {

        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Services to the container

            //services.AddMediatR(config =>
            //{
            //    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            //});

            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            var connectionstring = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<CatalogDbContext>((sp,options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(connectionstring);
            });

            services.AddScoped<IDataSeeder, CatalogDataSeeder>();

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
