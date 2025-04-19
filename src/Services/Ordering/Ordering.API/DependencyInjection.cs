using Carter;

namespace Ordering.API;

public static class DependencyInjection
{
    //Antes del build la aplicacion
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        //services.AddCarter();

        return services;
    }
    //Despues del buil de la aplicacion 
    public static WebApplication UseApiServices(this WebApplication app)
    {
        //app.MapCarter();

        return app;
    }
}