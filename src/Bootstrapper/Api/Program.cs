
namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services to Container
            builder.Services
                .AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);

            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");

            app
                .UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

            app.Run();
        }
    }
}
