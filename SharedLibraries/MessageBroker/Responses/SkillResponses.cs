using MessageBroker.Interfaces;

namespace SagaCoordinator.Domain.ResponseModels;

public class Skill
{
    public Guid SagaId { get; set; }
    public string SkillId { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }
}

public class SkillListResponse : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public List<Skill> Skills { get; set; }
    public string MessageType => nameof(SkillListResponse);
}