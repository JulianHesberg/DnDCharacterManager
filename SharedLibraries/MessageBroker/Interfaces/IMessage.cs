namespace MessageBroker.Interfaces;

public interface IMessage
{
    string MessageType { get; }
}