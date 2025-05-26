using MessageBroker.Configuration;
using MessageBroker.Factories;
using MessageBroker.Interfaces;
using SkillMicroservice.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

var rabbitMqOptions = builder.Configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();

// Register IMessageBroker using the factory
builder.Services.AddSingleton<IMessageBroker>(_ => RabbitMQFactory.Create(rabbitMqOptions));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
