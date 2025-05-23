namespace MessageBroker.Interfaces;

public interface IMessageHandler
{
    Task HandleMessageAsync(IMessage message);
}