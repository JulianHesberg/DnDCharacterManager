using MessageBroker;
using MessageBroker.Configuration;
using MessageBroker.Interfaces;
using MessageBroker.Requests;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace ItemMicroservice.Infrastructure.Services;

public class RabbitMQListener : BackgroundService
{
    private readonly IMessageBroker _messageBroker;
    private readonly IMessageHandler _messageHandler;
    
    public RabbitMQListener(IMessageBroker messageBroker, IMessageHandler messageHandler)
    {
        _messageBroker = messageBroker;
        _messageHandler = messageHandler;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageBroker.Subscribe(QueueNames.ItemServiceQueueIn, HandleMessage, stoppingToken);
        await _messageBroker.Subscribe(QueueNames.CompensationQueue, HandleMessage, stoppingToken);
    }

    private async void HandleMessage(IMessage message)
    {
        await _messageHandler.HandleMessageAsync(message);
    }

}