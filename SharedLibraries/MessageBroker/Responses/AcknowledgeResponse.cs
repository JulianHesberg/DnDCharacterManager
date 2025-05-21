namespace SagaCoordinator.Domain.ResponseModels;

public class AcknowledgeResponse
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public bool IsAcknowledged { get; set; }
}

public class RequestFailed
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ErrorMessage { get; set; }
}

public class RollbackCompleted
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
}

public class NotifyFailureToCharacter
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ErrorMessage { get; set; }
}