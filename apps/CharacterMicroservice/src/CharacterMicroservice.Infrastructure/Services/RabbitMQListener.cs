using MessageBroker;
using MessageBroker.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CharacterMicroservice.Infrastructure.Services;

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
        await _messageBroker.Subscribe(QueueNames.CharacterServiceQueueIn, HandleMessage, stoppingToken);
        await _messageBroker.Subscribe(QueueNames.CharacterCompensationQueueIn, HandleMessage, stoppingToken);
    }

    private async void HandleMessage(IMessage message)
    {
        await _messageHandler.HandleMessageAsync(message);
    }
}