using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;
public static class Extentions
{
    //Metodos de extension para configurar masstransit
    public static IServiceCollection AddMessageBroker
        //Se envia el assembly como opcional ya quen o es necesario para el que publica eventos
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            //Establece la nomenclatura de los nombres de los endpoints para que los nombres de las colas sean
            //del tipo order-created-event en vez de OrderCreatedEvent
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                //Registra los consumidores para su identificacion
                config.AddConsumers(assembly);

            //Configura Rabbit como bus de mensajeria
            config.UsingRabbitMq((context, configurator) =>
            {
                //Obtiene las configuraciones de conexion de los appsettings
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });
                // Busca los consumidores registrados y configura los endpoints correspondientes para los consumidores.
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}