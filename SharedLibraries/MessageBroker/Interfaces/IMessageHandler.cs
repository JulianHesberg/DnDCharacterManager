namespace MessageBroker.Interfaces;

public interface IMessageHandler
{
    Task HandleMessageAsync<T>(T message);
}