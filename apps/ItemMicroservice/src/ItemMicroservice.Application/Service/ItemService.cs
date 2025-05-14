using System;
using ItemMicroservice.Application.Service.Interfaces;
using ItemMicroservice.Domain.Entities;
using ItemMicroService.Application.Interfaces;

namespace ItemMicroservice.Application.Service;

public class ItemService : IItemService
{

    private readonly IItemRepository _repo;

    public ItemService(IItemRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Item>> GetAllAsync() => _repo.GetAllItemsAsync();
    
    public Task<Item?> GetByIdAsync(string id) => _repo.GetItemByIdAsync(id);

    public Task CreateAsync(Item item) => _repo.CreateAsync(item);

    public Task UpdateAsync(Item item) => _repo.UpdateAsync(item);

    public Task DeleteAsync(string id) => _repo.DeleteAsync(id);

}
