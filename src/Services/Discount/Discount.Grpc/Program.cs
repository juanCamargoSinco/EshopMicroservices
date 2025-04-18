using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
//Definicion del contexto de entity framework core
builder.Services.AddDbContext<DiscountContext>(options =>
{
    //Se usa sqllite como BD el cual esta integrado/embebido dentro del proyecto
    options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
});

//Configuracion para probar desde postman sin importar archivo. Requiere instalar el Grpc.AspNetCore.Server.Reflection
builder.Services.AddGrpcReflection();

var app = builder.Build();
//Configuracion para asegurar la migracion de la BD al iniciar el app y antes de aceptar cualquier peticion
app.UseMigration();

//Configuracion para probar desde postman sin importar archivo
if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

// Configure the HTTP request pipeline.

app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
