using ItemMicroservice.Application.Service;
using ItemMicroservice.Application.Service.Interfaces;
using ItemMicroservice.Infrastructure.Configurations;
using ItemMicroservice.Infrastructure.Repositories;
using ItemMicroService.Application.Interfaces;
using ItemMicroservice.Infrastructure.Services;
using MessageBroker.Configuration;
using MessageBroker.Implementations;
using MessageBroker.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register MongoDbSettings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQSettings"));


builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddSingleton<IMessageBroker, RabbitMqMessageBroker>();
builder.Services.AddSingleton<IMessageHandler, ItemMessageHandler>();

builder.Services.AddHostedService<RabbitMQListener>();

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