using System;
using ItemMicroservice.Application.Interfaces;
using ItemMicroservice.Application.Service.Interfaces;
using ItemMicroservice.Domain.Entities;
using ItemMicroService.Application.Interfaces;

namespace ItemMicroservice.Application.Service;

public class ItemService : IItemService
{

    private readonly IItemRepository _repo;
    private readonly IRabbitMQPublisher _publisher;

    public ItemService(IItemRepository repo, IRabbitMQPublisher publisher)
    {
        _repo = repo;
        _publisher = publisher;
    }

    public Task<List<Item>> GetAllAsync() => _repo.GetAllItemsAsync();

    public Task<Item?> GetByIdAsync(string id) => _repo.GetItemByIdAsync(id);

    public async Task CreateAsync(Item item)
    {
        // 1) Persist the new item
        await _repo.CreateAsync(item);

        // 2) Publish the created-item event
        var msg = new ItemCreatedMessage
        {
            Id    = item.Id,     // whatever type Id is (string/ObjectId/int)
            Name  = item.Name,
            Price = item.Price
        };
        await _publisher.PublishItemCreated(msg);
    }

    public Task UpdateAsync(Item item) => _repo.UpdateAsync(item);

    public Task DeleteAsync(string id) => _repo.DeleteAsync(id);

}
