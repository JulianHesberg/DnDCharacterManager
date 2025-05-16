using MessageBroker.Interfaces;
using SagaCoordinator.Domain.RequestModels;
using SagaCoordinator.Domain.ResponseModels;
using SagaCoordinator.Domain.SagaModels;
using SagaCoordinator.Infrastructure.Interfaces;

namespace SagaCoordinator.Application;

public class SagaMessageCoordinator
{
    private readonly ISagaRepository<PurchaseItemSaga> _purchaseItemSagaRepository;
    private readonly ISagaRepository<SellItemSaga> _sellItemSagaRepository;
    private readonly ISagaRepository<LevelUpSaga> _levelUpSagaRepository;
    private readonly IMessageBroker _messageBroker;
    
    public SagaMessageCoordinator(
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
        _messageBroker.Subscribe<PurchaseItemRequest>("CharacterServiceQueue", HandlePurchaseItemRequest);  
        _messageBroker.Subscribe<ItemListResponse>("ItemServiceQueue", HandleItemListResponse);  
          
        _messageBroker.Subscribe<SellItemRequest>("CharacterServiceQueue", HandleSellItemRequest);  
        _messageBroker.Subscribe<ItemCostResponse>("ItemServiceQueue", HandleItemCostResponse);  
          
        _messageBroker.Subscribe<LevelUpRequest>("CharacterServiceQueue", HandleLevelUpRequest);  
        _messageBroker.Subscribe<SkillListResponse>("SkillsServiceQueue", HandleSkillListResponse);  
  
        _messageBroker.Subscribe<AcknowledgeResponse>("CharacterServiceQueue", HandleAcknowledgeResponse);  
    }
    
    private void HandlePurchaseItemRequest(PurchaseItemRequest request)
    {
        var purchaseSaga = new PurchaseItemSaga
        {
            CharacterId = request.CharacterId,
            GoldAmount = request.GoldAmount,
            State = SagaState.Initialized
        };
        _purchaseItemSagaRepository.Save(purchaseSaga);
        _messageBroker.Publish("ItemServiceQueue", request);
    }

    private void HandleItemListResponse(ItemListResponse response)
    {
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.CharacterId);
        if(purchaseSaga != null)
        {
            purchaseSaga.State = SagaState.InProgress;
            _purchaseItemSagaRepository.Update(purchaseSaga);
            _messageBroker.Publish("CharacterServiceQueue", response);
        }
    }
    
    private void HandleSellItemRequest(SellItemRequest request)
    {
        var sellSaga = new SellItemSaga
        {
            CharacterId = request.CharacterId,
            ItemId = request.ItemId,
            State = SagaState.Initialized
        };
        _sellItemSagaRepository.Save(sellSaga);
        _messageBroker.Publish("ItemServiceQueue", request);
    }

    private void HandleItemCostResponse(ItemCostResponse response)
    {
        var sellSaga = _sellItemSagaRepository.FindById(response.CharacterId);
        if(sellSaga != null)
        {
            sellSaga.State = SagaState.InProgress;
            _sellItemSagaRepository.Update(sellSaga);
            _messageBroker.Publish("CharacterServiceQueue", response);
        }
    }
    
    private void HandleLevelUpRequest(LevelUpRequest request)
    {
        var levelUpSaga = new LevelUpSaga
        {
            CharacterId = request.CharacterId,
            State = SagaState.Initialized
        };
        _levelUpSagaRepository.Save(levelUpSaga);
        _messageBroker.Publish("SkillsServiceQueue", request);
    }
    
    private void HandleSkillListResponse(SkillListResponse response)
    {
        var levelUpSaga = _levelUpSagaRepository.FindById(response.CharacterId);
        if(levelUpSaga != null)
        {
            levelUpSaga.State = SagaState.InProgress;
            _levelUpSagaRepository.Update(levelUpSaga);
            _messageBroker.Publish("CharacterServiceQueue", response);
        }
    }

    private void HandleAcknowledgeResponse(AcknowledgeResponse response)
    {
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.CharacterId);
        if (response != null && response.IsAcknowledged)
        {
            purchaseSaga.State = SagaState.Completed;
            _purchaseItemSagaRepository.Update(purchaseSaga);
            return;
        }
        var sellSaga = _sellItemSagaRepository.FindById(response.CharacterId);
        if(response != null && response.IsAcknowledged)
        {
            sellSaga.State = SagaState.Completed;
            _sellItemSagaRepository.Update(sellSaga);
            return;
        }
        var levelUpSaga = _levelUpSagaRepository.FindById(response.CharacterId);
        if(response != null && response.IsAcknowledged)
        {
            levelUpSaga.State = SagaState.Completed;
            _levelUpSagaRepository.Update(levelUpSaga);
        }
    }
    
}