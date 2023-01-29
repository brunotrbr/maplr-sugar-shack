using maplr_api.Context;
using maplr_api.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using maplr_api.Interfaces;
using maplr_api.Repository;
using maplr_api.BusinessLayers;
using Microsoft.OpenApi.Models;
using maplr_api.Authentication;
using Microsoft.AspNetCore.Authentication;

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
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuth", Version = "v1" });
    config.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "basic"
                    }
                },
                new string[] {}
        }
    });
});

#region Adding Automapper to convert from Models to DTO
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Repository Dependency Injection

builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddScoped(typeof(ICartRepository), typeof(CartRepository));
builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));

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


builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddScoped<IAuthenticate, Authenticate>();  

//builder.Services.AddAuthentication(options =>
//{
//    options.AddScheme<AuthenticationHandler>("Basic", null); // Pode ser que essa linha não seja necessária
//    options.DefaultAuthenticateScheme = "Basic";
//    options.DefaultChallengeScheme = "Basic";
//});
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

app.UseRouting();
app.UseAuthentication();
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
