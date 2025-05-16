using SagaCoordinator.Domain.SagaModels;

namespace SagaCoordinator.Domain.Interfaces;

public interface ISaga
{
    Guid SagaId { get; }
    int CharacterId { get; }
    SagaState State { get; set; }
}