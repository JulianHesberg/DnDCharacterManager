using MessageBroker.Interfaces;
using MessageBroker.Requests;
using SagaCoordinator.Domain.ResponseModels;
using SagaCoordinator.Domain.SagaModels;
using SagaCoordinator.Infrastructure.Interfaces;

namespace SagaCoordinator.Application;

public class SagaMessageCoordinator
{
    private readonly ISagaRepository<PurchaseItemSaga> _purchaseItemSagaRepository;
    private readonly ISagaRepository<SellItemSaga> _sellItemSagaRepository;
    private readonly ISagaRepository<CraftItemSaga> _craftItemSagaRepository;
    private readonly ISagaRepository<LevelUpSaga> _levelUpSagaRepository;
    private readonly IMessageBroker _messageBroker;
    
    public SagaMessageCoordinator(
        ISagaRepository<PurchaseItemSaga> purchaseItemSagaRepository,
        ISagaRepository<SellItemSaga> sellItemSagaRepository,
        ISagaRepository<CraftItemSaga> craftItemSagaRepository,
        ISagaRepository<LevelUpSaga> levelUpSagaRepository,
        IMessageBroker messageBroker)
    {
        _purchaseItemSagaRepository = purchaseItemSagaRepository;
        _sellItemSagaRepository = sellItemSagaRepository;
        _craftItemSagaRepository = craftItemSagaRepository;
        _levelUpSagaRepository = levelUpSagaRepository;
        _messageBroker = messageBroker;
    }

    public void StartListening()  
    {  
        _messageBroker.Subscribe<PurchaseItemRequest>("CharacterServiceQueue", HandlePurchaseItemRequest);  
        _messageBroker.Subscribe<ItemListResponse>("ItemServiceQueue", HandleItemListResponse);  
          
        _messageBroker.Subscribe<SellItemRequest>("CharacterServiceQueue", HandleSellItemRequest);  
        _messageBroker.Subscribe<ItemCostResponse>("ItemServiceQueue", HandleItemCostResponse);  
        
        _messageBroker.Subscribe<CraftItemRequest>("CharacterServiceQueue", HandleCraftItemRequest);
        _messageBroker.Subscribe<ItemCraftedResponse>("ItemServiceQueue", HandleItemCraftedResponse);
          
        _messageBroker.Subscribe<LevelUpRequest>("CharacterServiceQueue", HandleLevelUpRequest);  
        _messageBroker.Subscribe<SkillListResponse>("SkillsServiceQueue", HandleSkillListResponse);  
  
        _messageBroker.Subscribe<AcknowledgeResponse>("CharacterServiceQueue", HandleAcknowledgeResponse);  
    }
    
    private void HandleCraftItemRequest(CraftItemRequest request)
    {
        var craftSaga = new CraftItemSaga
        {
            SagaId = Guid.NewGuid(),
            CharacterId = request.CharacterId,
            Price = request.Price,
            Name = request.Name,
            Description = request.Description,
            State = SagaState.Initialized
        };
        _craftItemSagaRepository.Save(craftSaga);
        _messageBroker.Publish("ItemServiceQueue", request);
    }
    
    private void HandleItemCraftedResponse(ItemCraftedResponse response)
    {
        var craftSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if(craftSaga != null)
        {
            craftSaga.ItemId = response.ItemId;
            craftSaga.State = SagaState.InProgress;
            _craftItemSagaRepository.Update(craftSaga);
            _messageBroker.Publish("CharacterServiceQueue", response);
        }
    }
    
    private void HandlePurchaseItemRequest(PurchaseItemRequest request)
    {
        var purchaseSaga = new PurchaseItemSaga
        {
            SagaId = new Guid(),
            CharacterId = request.CharacterId,
            GoldAmount = request.GoldAmount,
            State = SagaState.Initialized
        };
        _purchaseItemSagaRepository.Save(purchaseSaga);
        _messageBroker.Publish("ItemServiceQueue", request);
    }

    private void HandleItemListResponse(ItemListResponse response)
    {
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.SagaId);
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
            SagaId = new Guid(),
            CharacterId = request.CharacterId,
            ItemId = request.ItemId,
            State = SagaState.Initialized
        };
        _sellItemSagaRepository.Save(sellSaga);
        _messageBroker.Publish("ItemServiceQueue", request);
    }

    private void HandleItemCostResponse(ItemCostResponse response)
    {
        var sellSaga = _sellItemSagaRepository.FindById(response.SagaId);
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
            SagaId = new Guid(),
            CharacterId = request.CharacterId,
            Level = request.Level,
            State = SagaState.Initialized
        };
        _levelUpSagaRepository.Save(levelUpSaga);
        _messageBroker.Publish("SkillsServiceQueue", request);
    }
    
    private void HandleSkillListResponse(SkillListResponse response)
    {
        var levelUpSaga = _levelUpSagaRepository.FindById(response.SagaId);
        if(levelUpSaga != null)
        {
            levelUpSaga.State = SagaState.InProgress;
            _levelUpSagaRepository.Update(levelUpSaga);
            _messageBroker.Publish("CharacterServiceQueue", response);
        }
    }

    private void HandleAcknowledgeResponse(AcknowledgeResponse response)
    {
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.SagaId);
        if (response != null && response.IsAcknowledged)
        {
            purchaseSaga.State = SagaState.Completed;
            _purchaseItemSagaRepository.Update(purchaseSaga);
            return;
        }
        var sellSaga = _sellItemSagaRepository.FindById(response.SagaId);
        if(response != null && response.IsAcknowledged)
        {
            sellSaga.State = SagaState.Completed;
            _sellItemSagaRepository.Update(sellSaga);
            return;
        }
        var craftSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if(response != null && response.IsAcknowledged)
        {
            craftSaga.State = SagaState.Completed;
            _craftItemSagaRepository.Update(craftSaga);
            return;
        }
        var levelUpSaga = _levelUpSagaRepository.FindById(response.SagaId);
        if(response != null && response.IsAcknowledged)
        {
            levelUpSaga.State = SagaState.Completed;
            _levelUpSagaRepository.Update(levelUpSaga);
        }
    }
    
}