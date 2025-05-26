using MessageBroker.Interfaces;

namespace MessageBroker.Requests;

public class LevelUpRequest : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public int Level { get; set; }
    public string MessageType => nameof(LevelUpRequest);
}