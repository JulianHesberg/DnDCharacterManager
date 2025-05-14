using ItemMicroservice.Application.Interfaces;
using ItemMicroservice.Application.Service;
using ItemMicroservice.Application.Service.Interfaces;
using ItemMicroservice.Infrastructure.Configurations;
using ItemMicroservice.Infrastructure.Repositories;
using ItemMicroService.Application.Interfaces;
using ItemMicroService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register MongoDbSettings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// 1) Bind your RabbitMQSettings from appsettings.json
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

// 2) Pull the plain POCO out of IConfiguration
var rabbitConfig = builder.Configuration
    .GetSection("RabbitMQ")
    .Get<RabbitMQSettings>()!;

// 3) Wrap it in an IOptions<RabbitMQSettings>
var rabbitOptions = Options.Create(rabbitConfig);

// 4) Await the async factory to get a fully ready publisher
var rabbitPublisher = await RabbitMQPublisher.CreateAsync(rabbitOptions);

// 5) Register it in DI via the factory‐overload so there’s no overload conflict
builder.Services.AddSingleton<IRabbitMQPublisher>(_ => rabbitPublisher);

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();