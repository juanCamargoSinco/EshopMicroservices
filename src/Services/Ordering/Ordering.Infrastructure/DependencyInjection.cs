using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class DependencyInjection
{
    //IConfiguration para acceder a las cadenas de conexion
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Se obtiene la cadena de conexion de Ordering.API
        var connectionString = configuration.GetConnectionString("Database");

        //services.AddDbContext<ApplicationDbContext>((sp, options) =>
        //{
        //    options.UseSqlServer(connectionString);
        //});

        //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
