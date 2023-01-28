using maplr_api.Context;
using maplr_api.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using maplr_api.Interfaces;
using maplr_api.Repository;
using maplr_api.BusinessLayers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilter)))
    // Convert int to strings in enum on swagger
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    // Convert int to strings in enum on swagger
    config.SchemaFilter<EnumSchemaFilter>();
});

#region Adding Automapper to convert from Models to DTO
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Repository Dependency Injection

builder.Services.AddScoped(typeof(IProductRepository),typeof(ProductRepository));
builder.Services.AddScoped(typeof(ICartRepository),typeof(CartRepository));
builder.Services.AddScoped(typeof(IOrderRepository),typeof(OrderRepository));

#endregion

#region Adding Entity Framework Context and setting In-Memory Database

builder.Services.AddDbContext<MaplrContext>(options =>
options.UseInMemoryDatabase("MaplrDB"));

#endregion

#region Adding Data Generator

builder.Services.AddTransient<DataGenerator>();
#endregion

#region Adding Business Layers

builder.Services.AddScoped<OrdersBL>();
#endregion

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

//  Enale swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

#region Insert maple syrup data on startup
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = scopedFactory.CreateScope())
{
    var service = scope.ServiceProvider.GetService<DataGenerator>();
    service.InsertData();
}
#endregion

app.Run();
