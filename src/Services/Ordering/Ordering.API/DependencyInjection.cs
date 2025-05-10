using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    //Antes del build la aplicacion
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();

        //Adicion de manejo de excepciones personalizadas
        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database")!);

        return services;
    }
    //Despues del build de la aplicacion 
    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        //Configuracion de middleware para el manejo de excepciones
        //Al pasar una lambda vacía ({ }), no se configura ningún comportamiento de manejo de errores.
        app.UseExceptionHandler(options => { });

        //Configuracion para hacer el health check legible
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}