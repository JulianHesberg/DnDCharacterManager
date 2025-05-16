namespace MessageBroker.Requests;

public class LevelUpRequest
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public int Level { get; set; }
}