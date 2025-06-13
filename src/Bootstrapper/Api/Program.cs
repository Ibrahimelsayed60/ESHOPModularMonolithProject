
using Carter;
using Shared.Exceptions.Handler;
using Shared.Extensions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services to Container

            //builder.Services.AddCarter(configurator: config =>
            //{
            //    var catalogModules = typeof(CatalogModule).Assembly.GetTypes()
            //        .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

            //    config.WithModules(catalogModules);
            //});

            builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

            builder.Services
                .AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);

            builder.Services
                .AddExceptionHandler<CustomExceptionHandler>();

            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");

            app.MapCarter();

            app
                .UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

            app.UseExceptionHandler(options => { });

            app.Run();
        }
    }
}
