using MessageBroker.Configuration;
using MessageBroker.Factories;
using MessageBroker.Implementations;
using MessageBroker.Interfaces;
using SagaCoordinator.Application;
using SagaCoordinator.Domain.SagaModels;
using SagaCoordinator.Infrastructure.Implementations;
using SagaCoordinator.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var rabbitMqOptions = builder.Configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();

// Register IMessageBroker using the factory
builder.Services.AddScoped<IMessageBroker>(_ => RabbitMQFactory.Create(rabbitMqOptions));

// Replace with actual RabbitMQ credentials
builder.Services.AddScoped<ISagaRepository<PurchaseItemSaga>, InMemorySagaRepository<PurchaseItemSaga>>();
builder.Services.AddScoped<ISagaRepository<SellItemSaga>, InMemorySagaRepository<SellItemSaga>>();
builder.Services.AddScoped<ISagaRepository<LevelUpSaga>, InMemorySagaRepository<LevelUpSaga>>();
builder.Services.AddScoped<SagaMessageCoordinator>();

var app = builder.Build();

var sagaCoordinator = app.Services.GetRequiredService<SagaMessageCoordinator>();
sagaCoordinator.StartListening();


app.Run();