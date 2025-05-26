using MessageBroker;
using MessageBroker.Interfaces;
using MessageBroker.Requests;
using Microsoft.Extensions.Hosting;

namespace SkillMicroservice.Infrastructure.Services;

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
        await _messageBroker.Subscribe<CraftItemRequest>(QueueNames.ItemServiceQueue, async message =>
        {
            await _messageHandler.HandleMessageAsync(message);
        });
        
        await _messageBroker.Subscribe<RollbackItemCraftedRequest>(QueueNames.CompensationQueue, async message =>
        {
            await _messageHandler.HandleMessageAsync(message);
        });
    }
}