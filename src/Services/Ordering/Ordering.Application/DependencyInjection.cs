﻿using BuildingBlocks.Behaviors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application;

//Los metodos de extension debe ser static
public static class DependencyInjection
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        //services.AddFeatureManagement();
        //services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
