using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Cargar ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Agregar Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();
await app.UseOcelot();

app.Run();
