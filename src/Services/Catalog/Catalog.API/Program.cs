

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
// Add services to the container

//MediatR se usa para implementar el patron mediator, facilitando CQRS y para separar querys y commands
//Endpoint son los puntos de entrada
//Handler son los manejadores los que hacen la logica
builder.Services.AddMediatR(config =>
{
    //Se activa mediatR en el asembly actual
    config.RegisterServicesFromAssembly(assembly);
    //Añade el comportamiento de pipeline behavior en MediatR
    //typeof(ValidationBehavior<,>) Especifica el tipo genérico ValidationBehavior que acepta dos parámetros genéricos.
    //La notación <,> indica que es un tipo abierto con dos parámetros genéricos no especificados.
    //Añadimos ValidationBehavior como un pipeline behavior en mediatr
    //Los pipelines behavior se ejecutan en el orden en que esten aca definidos
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LogginBehavior<,>));
});

//Adicion de FluentValidation para que se busque cualquier validator en el proyecto/ensamblado
//mediante la configuracion typeof(Program).Assembly 
builder.Services.AddValidatorsFromAssembly(assembly);

//Carter sirve para extender el funcionamiento/mapeo de las minimal apis
builder.Services.AddCarter();

//Marten permite manejar postresql como una BD de documentos para almacenar json
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

//Mapster (que no esta aca) se usa para mapear :V

//Configuramos nuestro propio manejador de excepciones como servicio en la inyeccion de dependencias
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().
    AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

//Configuramos la app para usar el CustomExceptionHandler.
//Esto se hace al pasar como parametro algo vacio dentro de UseExceptionHandler
//indicando que usaremos una implementacion propia
app.UseExceptionHandler(x => { });

app.UseHealthChecks("/health"
    , new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
