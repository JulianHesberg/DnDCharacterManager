using SagaCoordinator.Domain.SagaModels;

namespace SagaCoordinator.Domain.Interfaces;

public interface ISaga
{
    int CharacterId { get; }
    SagaState State { get; set; }
}