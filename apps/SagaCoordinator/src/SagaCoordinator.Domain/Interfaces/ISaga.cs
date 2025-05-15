using SagaCoordinator.Domain.SagaModels;

namespace SagaCoordinator.Domain.Interfaces;

public interface ISaga
{
    Guid SagaId { get; }
    SagaState State { get; set; }
}