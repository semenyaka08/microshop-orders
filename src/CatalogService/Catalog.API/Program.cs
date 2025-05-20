using Catalog.API.Services;
using Catalog.BusinessLogic.Services;
using Catalog.DAL;
using Catalog.DAL.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1"
    });
});

builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<IProductService, ProductsService>();

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await dbContext.Database.MigrateAsync();

    var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await dataSeeder.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
    c.RoutePrefix = string.Empty; 
});

app.MapControllers();
app.MapGrpcService<CatalogGrpcService>();

app.Run();