
using ItemMicroservice.Domain.Entities;

namespace ItemMicroService.Application.Interfaces;

public interface IItemRepository
{
    Task<List<Item>> GetAllItemsAsync();
    Task<Item?> GetItemByIdAsync(string id);
    Task CreateAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(string id);
}
