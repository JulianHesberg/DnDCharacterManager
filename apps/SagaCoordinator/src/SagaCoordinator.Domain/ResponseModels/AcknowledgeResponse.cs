namespace SagaCoordinator.Domain.ResponseModels;

public class AcknowledgeResponse
{
    public int CharacterId { get; set; }
    public bool IsAcknowledged { get; set; }
}