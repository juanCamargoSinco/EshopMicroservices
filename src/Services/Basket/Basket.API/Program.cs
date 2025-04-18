using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

//Antes de hacer build de la aplicacion
//Adicion de servicios al container con inyeccion de dependecias
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    //Se activa mediatR en el asembly actual
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LogginBehavior<,>));
});


builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
})
//usando Marten existen diferentes tipos de sesiones, en la mayoria de casos se usa UseLightweightSessions
// sin embargo tener presente que hay otras dos que tienen sus respectivos escenarios
.UseLightweightSessions();

//Configuracion de la inyeccion de dependencias de los repositorios
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//Configuracion de redis como cache distribuida
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});

//Registro de manejador de excepciones personalizadas
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    //Configuracion de health check de postgres y redis
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();


//Redis es un almacen de datos en memoria y cache distribuido

// Configurar el HTTP request pipeline

app.MapCarter();
app.UseExceptionHandler(option => { });

app.UseHealthChecks("/health",
    //Adicion de respuesta en formato JSON de los healtchecks configurados
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
