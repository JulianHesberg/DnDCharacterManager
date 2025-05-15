using MessageBroker.Interfaces;
using SagaCoordinator.Domain.SagaModels;
using SagaCoordinator.Infrastructure.Interfaces;

namespace SagaCoordinator.Application;

public class SagaCoordinator
{
    private readonly ISagaRepository<PurchaseItemSaga> _purchaseItemSagaRepository;
    private readonly ISagaRepository<SellItemSaga> _sellItemSagaRepository;
    private readonly ISagaRepository<LevelUpSaga> _levelUpSagaRepository;
    private readonly IMessageBroker _messageBroker;
    
    public SagaCoordinator(
        ISagaRepository<PurchaseItemSaga> purchaseItemSagaRepository,
        ISagaRepository<SellItemSaga> sellItemSagaRepository,
        ISagaRepository<LevelUpSaga> levelUpSagaRepository,
        IMessageBroker messageBroker)
    {
        _purchaseItemSagaRepository = purchaseItemSagaRepository;
        _sellItemSagaRepository = sellItemSagaRepository;
        _levelUpSagaRepository = levelUpSagaRepository;
        _messageBroker = messageBroker;
    }

    public void StartListening()
    {
        
    }
}