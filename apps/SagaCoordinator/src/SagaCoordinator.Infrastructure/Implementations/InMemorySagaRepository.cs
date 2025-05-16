using System.Collections.Concurrent;
using SagaCoordinator.Domain.Interfaces;
using SagaCoordinator.Infrastructure.Interfaces;

namespace SagaCoordinator.Infrastructure.Implementations;

public class InMemorySagaRepository<TSaga> : ISagaRepository<TSaga> where TSaga : ISaga
{
    private readonly ConcurrentDictionary<int, TSaga> _sagas = new();
    
    public void Save(TSaga saga)
    {
        _sagas[saga.CharacterId] = saga;
    }

    public void Update(TSaga saga)
    {
        if(_sagas.ContainsKey(saga.CharacterId))
        {
            _sagas[saga.CharacterId] = saga;
        }
        else
        {
            throw new KeyNotFoundException($"Saga with ID {saga.CharacterId} not found.");
        }
    }

    public TSaga FindById(int sagaId)
    {
        _sagas.TryGetValue(sagaId, out var saga);  
        return saga;  
    }
}