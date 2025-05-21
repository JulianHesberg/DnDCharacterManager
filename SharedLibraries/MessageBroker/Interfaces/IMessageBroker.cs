namespace MessageBroker.Interfaces;

public interface IMessageBroker
{
    Task Publish<T>(string queueName, T message);
    Task Subscribe<T>(string queueName, Func<T, Task> onMessageRecieved);
}