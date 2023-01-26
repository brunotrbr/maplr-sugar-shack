using maplr_api.Context;
using maplr_api.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using maplr_api.Interfaces;
using maplr_api.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SchemaFilter<EnumSchemaFilter>();
});

#region Repository Dependency Injection

builder.Services.AddScoped(typeof(IMapleSyrupRepository),typeof(MapleSyrupRepository));

#endregion

#region Adding Entity Framework Context and setting In-Memory Database

builder.Services.AddDbContext<MaplrContext>(options =>
options.UseInMemoryDatabase("MaplrDB"));

#endregion

#region Adding Data Generator

builder.Services.AddTransient<DataGenerator>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
