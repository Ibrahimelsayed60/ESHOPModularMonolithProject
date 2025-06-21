
using Carter;
using Keycloak.AuthServices.Authentication;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extensions;
using Shared.Messaging.Extensions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(context.Configuration));

            // Add Services to Container

            //builder.Services.AddCarter(configurator: config =>
            //{
            //    var catalogModules = typeof(CatalogModule).Assembly.GetTypes()
            //        .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

            //    config.WithModules(catalogModules);
            //});

            //common services: carter, mediatr, fluentvalidation
            var catalogAssembly = typeof(CatalogModule).Assembly;
            var basketAssembly = typeof(BasketModule).Assembly;
            var orderingAssembly = typeof(OrderingModule).Assembly;

            builder.Services
                .AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

            builder.Services
                .AddMediatRWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            builder.Services
                .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly, orderingAssembly);

            builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
            builder.Services.AddAuthorization();

            //module services: catalog, basket, ordering
            builder.Services
                .AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);

            builder.Services
                .AddExceptionHandler<CustomExceptionHandler>();


            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");

            app.MapCarter();

            app.UseSerilogRequestLogging();

            app.UseExceptionHandler(options => { });

            app.UseAuthentication();
            app.UseAuthorization();


            app
                .UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

            

            app.Run();
        }
    }
}
