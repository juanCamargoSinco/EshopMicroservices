using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//Adicion de servicios al container

//Configuracion proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        //En diez segundos se admite un maximo de 5 solicitudes
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

//Configurar el HTTP request pipeline

//Configuracion proxy
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();
