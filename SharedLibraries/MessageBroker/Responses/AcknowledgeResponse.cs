using MessageBroker.Interfaces;

namespace SagaCoordinator.Domain.ResponseModels;

public class AcknowledgeResponse : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public bool IsAcknowledged { get; set; }
    public string MessageType => nameof(AcknowledgeResponse);
}

public class RequestFailed : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ErrorMessage { get; set; }
    public string MessageType => nameof(RequestFailed);
}

public class RollbackCompleted : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string MessageType => nameof(RollbackCompleted);
}

public class NotifyFailureToCharacter : IMessage
{
    public Guid SagaId { get; set; }
    public int CharacterId { get; set; }
    public string ErrorMessage { get; set; }
    public string MessageType => nameof(NotifyFailureToCharacter);
}