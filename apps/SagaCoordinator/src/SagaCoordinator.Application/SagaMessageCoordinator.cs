using MessageBroker;
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
        _messageBroker.Subscribe<PurchaseItemRequest>(QueueNames.CharacterServiceQueue, HandlePurchaseItemRequest);  
        _messageBroker.Subscribe<ItemListResponse>(QueueNames.ItemServiceQueue, HandleItemListResponse);  
          
        _messageBroker.Subscribe<SellItemRequest>(QueueNames.CharacterServiceQueue, HandleSellItemRequest);  
        _messageBroker.Subscribe<ItemCostResponse>(QueueNames.ItemServiceQueue, HandleItemCostResponse);  
        
        _messageBroker.Subscribe<CraftItemRequest>(QueueNames.CharacterServiceQueue, HandleCraftItemRequest);
        _messageBroker.Subscribe<ItemCraftedResponse>(QueueNames.ItemServiceQueue, HandleItemCraftedResponse);
          
        _messageBroker.Subscribe<LevelUpRequest>(QueueNames.CharacterServiceQueue, HandleLevelUpRequest);  
        _messageBroker.Subscribe<SkillListResponse>(QueueNames.SkillServiceQueue, HandleSkillListResponse);  
  
        _messageBroker.Subscribe<AcknowledgeResponse>(QueueNames.CharacterServiceQueue, HandleAcknowledgeResponse); 
        
        _messageBroker.Subscribe<RequestFailed>(QueueNames.CompensationQueue, HandleRequestFailed);
    }
    
    private async Task HandleCraftItemRequest(CraftItemRequest request)
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
        await _messageBroker.Publish(QueueNames.ItemServiceQueue, request);
    }
    
    private async Task HandleItemCraftedResponse(ItemCraftedResponse response)
    {
        var craftSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if(craftSaga != null)
        {
            craftSaga.ItemId = response.ItemId;
            craftSaga.State = SagaState.InProgress;
            _craftItemSagaRepository.Update(craftSaga);
            await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }
    
    private async Task HandlePurchaseItemRequest(PurchaseItemRequest request)
    {
        var purchaseSaga = new PurchaseItemSaga
        {
            SagaId = new Guid(),
            CharacterId = request.CharacterId,
            GoldAmount = request.GoldAmount,
            State = SagaState.Initialized
        };
        _purchaseItemSagaRepository.Save(purchaseSaga);
        await _messageBroker.Publish(QueueNames.ItemServiceQueue, request);
    }

    private async Task HandleItemListResponse(ItemListResponse response)
    {
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.SagaId);
        if(purchaseSaga != null)
        {
            purchaseSaga.State = SagaState.InProgress;
            _purchaseItemSagaRepository.Update(purchaseSaga);
           await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }
    
    private async Task HandleSellItemRequest(SellItemRequest request)
    {
        var sellSaga = new SellItemSaga
        {
            SagaId = new Guid(),
            CharacterId = request.CharacterId,
            ItemId = request.ItemId,
            State = SagaState.Initialized
        };
        _sellItemSagaRepository.Save(sellSaga);
        await _messageBroker.Publish(QueueNames.ItemServiceQueue, request);
    }

    private async Task HandleItemCostResponse(ItemCostResponse response)
    {
        var sellSaga = _sellItemSagaRepository.FindById(response.SagaId);
        if(sellSaga != null)
        {
            sellSaga.State = SagaState.InProgress;
            _sellItemSagaRepository.Update(sellSaga);
            await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }
    
    private async Task HandleLevelUpRequest(LevelUpRequest request)
    {
        var levelUpSaga = new LevelUpSaga
        {
            SagaId = new Guid(),
            CharacterId = request.CharacterId,
            Level = request.Level,
            State = SagaState.Initialized
        };
        _levelUpSagaRepository.Save(levelUpSaga);
        await _messageBroker.Publish(QueueNames.SkillServiceQueue, request);
    }
    
    private async Task HandleSkillListResponse(SkillListResponse response)
    {
        var levelUpSaga = _levelUpSagaRepository.FindById(response.SagaId);
        if(levelUpSaga != null)
        {
            levelUpSaga.State = SagaState.InProgress;
            _levelUpSagaRepository.Update(levelUpSaga);
            await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }

    private async Task HandleAcknowledgeResponse(AcknowledgeResponse response)
    {
        // Acknowledge Item Purchase
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.SagaId);
        if (purchaseSaga != null && response.IsAcknowledged)
        {
            purchaseSaga.State = SagaState.Completed;
            _purchaseItemSagaRepository.Update(purchaseSaga);
            return;
        }
        
        // Acknowledge Item Sell
        var sellSaga = _sellItemSagaRepository.FindById(response.SagaId);
        if(sellSaga != null && response.IsAcknowledged)
        {
            sellSaga.State = SagaState.Completed;
            _sellItemSagaRepository.Update(sellSaga);
            return;
        }
        
        // Acknowledge Item Craft
        var craftSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if(craftSaga != null && response.IsAcknowledged)
        {
            craftSaga.State = SagaState.Completed;
            _craftItemSagaRepository.Update(craftSaga);
            return;
        }
        
        // Acknowledge Level Up
        var levelUpSaga = _levelUpSagaRepository.FindById(response.SagaId);
        if(levelUpSaga != null && response.IsAcknowledged)
        {
            levelUpSaga.State = SagaState.Completed;
            _levelUpSagaRepository.Update(levelUpSaga);
        }
    }

    private async Task HandleRequestFailed(RequestFailed response)
    {
        var itemSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if(itemSaga != null)
        {
            itemSaga.State = SagaState.Failed;
            _craftItemSagaRepository.Update(itemSaga);
            var rollback = new RollbackItemCraftedRequest
            {
                SagaId = itemSaga.SagaId,
                ItemId = itemSaga.ItemId
            };
            await _messageBroker.Publish(QueueNames.CompensationQueue, rollback);
            return;
        }
        
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.SagaId);
        if (purchaseSaga != null)
        {
            purchaseSaga.State = SagaState.Failed;
            _purchaseItemSagaRepository.Update(purchaseSaga);
            var errorMessage = new NotifyFailureToCharacter
            {
                SagaId = purchaseSaga.SagaId,
                CharacterId = purchaseSaga.CharacterId,
                ErrorMessage = response.ErrorMessage
            };
            await _messageBroker.Publish(QueueNames.CompensationQueue, errorMessage);
        }
        
        var sellSaga = _sellItemSagaRepository.FindById(response.SagaId);
        if (sellSaga != null)
        {
            sellSaga.State = SagaState.Failed;
            _sellItemSagaRepository.Update(sellSaga);
            var errorMessage = new NotifyFailureToCharacter
            {
                SagaId = sellSaga.SagaId,
                CharacterId = sellSaga.CharacterId,
                ErrorMessage = response.ErrorMessage
            };
            await _messageBroker.Publish(QueueNames.CompensationQueue, errorMessage);
        }
        
        var levelUpSaga = _levelUpSagaRepository.FindById(response.SagaId);
        if (levelUpSaga != null)
        {
            levelUpSaga.State = SagaState.Failed;
            _levelUpSagaRepository.Update(levelUpSaga);
            var errorMessage = new NotifyFailureToCharacter
            {
                SagaId = levelUpSaga.SagaId,
                CharacterId = levelUpSaga.CharacterId,
                ErrorMessage = response.ErrorMessage
            };
            await _messageBroker.Publish(QueueNames.CompensationQueue, errorMessage);
        }
    }
    
}