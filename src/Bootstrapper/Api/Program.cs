
using Carter;
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

            builder.Services
                .AddCarterWithAssemblies(catalogAssembly, basketAssembly);

            builder.Services
                .AddMediatRWithAssemblies(catalogAssembly, basketAssembly);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            builder.Services
                .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly);

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

            app
                .UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

            

            app.Run();
        }
    }
}
