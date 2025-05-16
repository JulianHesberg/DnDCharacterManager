namespace SagaCoordinator.Domain.ResponseModels;

public class AcknowledgeResponse
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public bool IsAcknowledged { get; set; }
}