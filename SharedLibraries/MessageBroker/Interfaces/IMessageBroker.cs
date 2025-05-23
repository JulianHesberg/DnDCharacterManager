namespace MessageBroker.Interfaces;

public interface IMessageBroker
{
    Task Publish<T>(string queueName, T message, CancellationToken cancellationToken = default);
    Task Subscribe(string queueName, Action<IMessage> handler, CancellationToken cancellationToken = default);
    Task Unsubscribe<T>(string queueName, CancellationToken cancellationToken = default);
}