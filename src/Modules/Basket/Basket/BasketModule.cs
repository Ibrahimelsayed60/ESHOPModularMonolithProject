﻿using Basket.Data;
using Basket.Data.Processors;
using Basket.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Services to the container

            // Add services to the container.
            // 1. Api Endpoint services

            // 2. Application Use Case services
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Decorate<IBasketRepository, CachedBasketRepository>();

            // 3. Data - Infrastructure services
            var connectionString = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<BasketDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseNpgsql(connectionString);
            });

            services.AddHostedService<OutboxProcessor>();

            return services;
        }


        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {
            // 1. Use Api Endpoint services

            // 2. Use Application Use Case services

            // 3. Use Data - Infrastructure services
            app.UseMigration<BasketDbContext>();

            return app;
        }

    }
}
