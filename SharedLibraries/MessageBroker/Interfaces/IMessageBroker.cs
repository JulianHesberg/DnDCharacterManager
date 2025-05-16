namespace MessageBroker.Interfaces;

public interface IMessageBroker
{
    void Publish<T>(string queueName, T message);
    void Subscribe<T>(string queueName, Action<T> onMessageRecieved);
}