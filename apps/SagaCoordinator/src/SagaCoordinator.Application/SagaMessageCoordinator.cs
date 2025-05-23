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

    public async Task StartListening()
    {
        await _messageBroker.Subscribe(QueueNames.CharacterServiceQueue, HandleMessage);
        await _messageBroker.Subscribe(QueueNames.ItemServiceQueue, HandleMessage);
        await _messageBroker.Subscribe(QueueNames.SkillServiceQueue, HandleMessage);
        await _messageBroker.Subscribe(QueueNames.CompensationQueue, HandleMessage);
    }
    
    private void HandleMessage(IMessage message)
    {
        switch (message)
        {
            case PurchaseItemRequest purchase:
                HandlePurchaseItemRequest(purchase);
                break;
            case ItemListResponse itemList:
                HandleItemListResponse(itemList);
                break;
            case SellItemRequest sell:
                HandleSellItemRequest(sell);
                break;
            case ItemCostResponse itemCost:
                HandleItemCostResponse(itemCost);
                break;
            case CraftItemRequest craft:
                HandleCraftItemRequest(craft);
                break;
            case ItemCraftedResponse crafted:
                HandleItemCraftedResponse(crafted);
                break;
            case LevelUpRequest levelUp:
                HandleLevelUpRequest(levelUp);
                break;
            case SkillListResponse skillList:
                HandleSkillListResponse(skillList);
                break;
            case AcknowledgeResponse acknowledge:
                HandleAcknowledgeResponse(acknowledge);
                break;
            case RequestFailed requestFailed:
                HandleRequestFailed(requestFailed);
                break;
            case RollbackCompleted rollbackCompleted:
                HandleRollbackCompleted(rollbackCompleted);
                break;
            default:
                Console.WriteLine($"Unknown message type received: {message.GetType().Name}");
                break;
        }
    }
    private async void HandleCraftItemRequest(CraftItemRequest request)
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
    
    private async void HandleItemCraftedResponse(ItemCraftedResponse response)
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
    
    private async void HandlePurchaseItemRequest(PurchaseItemRequest request)
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

    private async void HandleItemListResponse(ItemListResponse response)
    {
        var purchaseSaga = _purchaseItemSagaRepository.FindById(response.SagaId);
        if(purchaseSaga != null)
        {
            purchaseSaga.State = SagaState.InProgress;
            _purchaseItemSagaRepository.Update(purchaseSaga);
           await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }
    
    private async void HandleSellItemRequest(SellItemRequest request)
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

    private async void HandleItemCostResponse(ItemCostResponse response)
    {
        var sellSaga = _sellItemSagaRepository.FindById(response.SagaId);
        if(sellSaga != null)
        {
            sellSaga.State = SagaState.InProgress;
            _sellItemSagaRepository.Update(sellSaga);
            await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }
    
    private async void HandleLevelUpRequest(LevelUpRequest request)
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
    
    private async void HandleSkillListResponse(SkillListResponse response)
    {
        var levelUpSaga = _levelUpSagaRepository.FindById(response.SagaId);
        if(levelUpSaga != null)
        {
            levelUpSaga.State = SagaState.InProgress;
            _levelUpSagaRepository.Update(levelUpSaga);
            await _messageBroker.Publish(QueueNames.CharacterServiceQueue, response);
        }
    }

    private void HandleAcknowledgeResponse(AcknowledgeResponse response)
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

    private async void HandleRequestFailed(RequestFailed response)
    {
        var itemSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if(itemSaga != null)
        {
            itemSaga.State = SagaState.InProgress;
            _craftItemSagaRepository.Update(itemSaga);
            var rollback = new RollbackItemCraftedRequest
            {
                SagaId = itemSaga.SagaId,
                CharacterId = response.CharacterId,
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

    private void HandleRollbackCompleted(RollbackCompleted response)
    {
        var craftedSaga = _craftItemSagaRepository.FindById(response.SagaId);
        if (craftedSaga.SagaId != null)
        {
            craftedSaga.State = SagaState.Failed;
            _craftItemSagaRepository.Update(craftedSaga);
        }
    }
}