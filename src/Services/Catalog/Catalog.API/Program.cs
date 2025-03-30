var builder = WebApplication.CreateBuilder(args);

// Add services to the container

//Carter sirve para extender el funcionamiento/mapeo de las minimal apis
builder.Services.AddCarter();

//MediatR se usa para implementar el patron mediator, facilitando CQRS y para separar querys y commands
//Endpoint son los puntos de entrada
//Handler son los manejadores los que hacen la logica
builder.Services.AddMediatR(config => {

    config.RegisterServicesFromAssembly(typeof(Program).Assembly);

});

//Marten permite manejar postres como una BD de documentos para almacenar json
builder.Services.AddMarten(opts => {

    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

//Mapster (que no esta aca) se usa para mapear :V

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();    

app.Run();
