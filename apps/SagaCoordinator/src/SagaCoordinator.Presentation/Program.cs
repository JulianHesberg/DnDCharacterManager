using MessageBroker.Implementations;
using MessageBroker.Interfaces;
using SagaCoordinator.Application;
using SagaCoordinator.Domain.SagaModels;
using SagaCoordinator.Infrastructure.Implementations;
using SagaCoordinator.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageBroker>(provider =>
    new RabbitMqMessageBroker("localhost", "guest", "guest")); // Replace with actual RabbitMQ credentials
builder.Services.AddScoped<ISagaRepository<PurchaseItemSaga>, InMemorySagaRepository<PurchaseItemSaga>>();
builder.Services.AddScoped<ISagaRepository<SellItemSaga>, InMemorySagaRepository<SellItemSaga>>();
builder.Services.AddScoped<ISagaRepository<LevelUpSaga>, InMemorySagaRepository<LevelUpSaga>>();
builder.Services.AddScoped<SagaMessageCoordinator>();

var app = builder.Build();

var sagaCoordinator = app.Services.GetRequiredService<SagaMessageCoordinator>();
sagaCoordinator.StartListening();


app.Run();