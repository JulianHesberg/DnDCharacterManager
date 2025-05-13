
using ItemMicroservice.Domain.Entities;

namespace ItemMicroService.Application.Interfaces;

public interface IItemsRepository
{
    Task<List<Items>> GetAllItemsAsync();
    Task<Items?> GetItemByIdAsync(int id);
    Task CreateAsync(Items item);
    Task UpdateAsync(Items item);
    Task DeleteAsync(int id);
}
