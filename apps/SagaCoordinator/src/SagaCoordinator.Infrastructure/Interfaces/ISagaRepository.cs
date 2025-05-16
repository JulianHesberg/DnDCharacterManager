using SagaCoordinator.Domain.Interfaces;

namespace SagaCoordinator.Infrastructure.Interfaces;

public interface ISagaRepository<TSaga> where TSaga : ISaga
{
    void Save(TSaga saga);
    void Update(TSaga saga);
    TSaga FindById(Guid sagaId);
}