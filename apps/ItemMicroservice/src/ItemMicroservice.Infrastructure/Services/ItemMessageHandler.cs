using ItemMicroService.Application.Interfaces;
using ItemMicroservice.Infrastructure.Repositories;
using MessageBroker;
using MessageBroker.Interfaces;
using MessageBroker.Requests;
using SagaCoordinator.Domain.ResponseModels;
using Item = ItemMicroservice.Domain.Entities.Item;

namespace ItemMicroservice.Infrastructure.Services;

public class ItemMessageHandler : IMessageHandler
{
    private readonly IMessageBroker _messageBroker;
    private readonly IItemRepository _repository;
    
    public ItemMessageHandler(IMessageBroker messageBroker, IItemRepository repository)
    {
        _repository = repository;
        _messageBroker = messageBroker;
    }
    public async Task HandleMessageAsync(IMessage message)
    {
        switch (message)
        {
            case CraftItemRequest craft:
                await HandleCraftItemRequest(craft);
                break;
            
            case RollbackItemCraftedRequest rollback:
                await HandleRollbackItemCraftedRequest(rollback);
                break;
        }
    }
    
    private async Task HandleCraftItemRequest(CraftItemRequest request)
    {
        var item = new Item
        { 
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };
        var created = await _repository.CreateAsync(item);
        if (created.Id != null)
        {
            var response = new ItemCraftedResponse
            {
                CharacterId = request.CharacterId,
                ItemId = created.Id,
                SagaId = request.SagaId
            };

            await _messageBroker.Publish(QueueNames.ItemServiceQueueOut, response);
        }
        else
        {
            var response = new RequestFailed
            {
                CharacterId = request.CharacterId,
                SagaId = request.SagaId,
                ErrorMessage = "Item creation failed"
            };
            await _messageBroker.Publish(QueueNames.ItemCompensationQueueOut, response);
        }
    }
    
    private async Task HandleRollbackItemCraftedRequest(RollbackItemCraftedRequest request)
    {
        var item = await _repository.GetItemByIdAsync(request.ItemId);
        if (item != null)
        {
            await _repository.DeleteAsync(item.Id);
            var rollback = new RollbackCompleted
            {
                SagaId = request.SagaId,
                CharacterId = request.CharacterId
            };
            await _messageBroker.Publish(QueueNames.ItemCompensationQueueOut, rollback);
        }
    }
}