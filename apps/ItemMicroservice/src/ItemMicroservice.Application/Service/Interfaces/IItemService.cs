using System;
using ItemMicroservice.Domain.Entities;

namespace ItemMicroservice.Application.Service.Interfaces;

public interface IItemService
{
    Task<List<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(string id);
    Task CreateAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(string id);
}
