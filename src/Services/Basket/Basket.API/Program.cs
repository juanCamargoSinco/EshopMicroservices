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
//Registro de manejador de excepciones personalizadas
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


//Redis es un almacen de datos en memoria y cache distribuido

// Configurar el HTTP request pipeline

app.MapCarter();
app.UseExceptionHandler(option => { });

app.Run();
