using System.Collections.Concurrent;
using SagaCoordinator.Domain.Interfaces;
using SagaCoordinator.Infrastructure.Interfaces;

namespace SagaCoordinator.Infrastructure.Implementations;

public class InMemorySagaRepository<TSaga> : ISagaRepository<TSaga> where TSaga : ISaga
{
    private readonly ConcurrentDictionary<Guid, TSaga> _sagas = new();
    
    public void Save(TSaga saga)
    {
        _sagas[saga.SagaId] = saga;
    }

    public void Update(TSaga saga)
    {
        if(_sagas.ContainsKey(saga.SagaId))
        {
            _sagas[saga.SagaId] = saga;
        }
        else
        {
            throw new KeyNotFoundException($"Saga with ID {saga.SagaId} not found.");
        }
    }

    public TSaga FindById(Guid sagaId)
    {
        _sagas.TryGetValue(sagaId, out var saga);  
        return saga;  
    }
}