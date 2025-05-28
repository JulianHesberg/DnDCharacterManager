using MessageBroker.Interfaces;

using EasyNetQ;
using IMessage = MessageBroker.Interfaces.IMessage;

namespace MessageBroker.Implementations;

public class RabbitMqMessageBroker : IMessageBroker
{
    private readonly IBus _bus;
    private readonly Dictionary<string, IDisposable> _subscriptions = new Dictionary<string, IDisposable>();
    
    public RabbitMqMessageBroker(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task Publish<T>(string queueName, T message, CancellationToken cancellationToken = default)
    {
        await _bus.PubSub.PublishAsync<T>(message, cancellationToken);
    }

    public async Task Subscribe(string queueName, Action<IMessage> onMessageReceived, CancellationToken cancellationToken = default)
    {
        if(_subscriptions.Keys.Contains(queueName)){
            throw new ArgumentException("Already Subscribed to this queue: " + queueName);
        }
        var subscriptionHandle = await _bus.PubSub.SubscribeAsync<IMessage>(
            queueName,
            message => onMessageReceived(message),
            cancellationToken
        );
        _subscriptions.Add(queueName, subscriptionHandle);
    }
    
    public Task Unsubscribe(string queueName, CancellationToken cancellationToken = default)  
    {  
        if(_subscriptions.TryGetValue(queueName, out var subscriptionHandle)){
            subscriptionHandle.Dispose();
            _subscriptions.Remove(queueName);
        }
        return Task.CompletedTask;
    }  
}