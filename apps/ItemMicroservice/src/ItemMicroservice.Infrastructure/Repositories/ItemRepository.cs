using ItemMicroService.Application.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ItemMicroservice.Domain.Entities;
using ItemMicroservice.Infrastructure.Configurations;

namespace ItemMicroservice.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly IMongoCollection<Item> _items;

    public ItemRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _items = database.GetCollection<Item>(settings.Value.ItemsCollectionName);
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        var result = await _items.Find(_ => true).ToListAsync();
        return result;
    }

    public async Task<Item?> GetItemByIdAsync(string id)
    {
        return await _items.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Item item)
    {
        await _items.InsertOneAsync(item);
    }
    public async Task UpdateAsync(Item item)
    {
        await _items.ReplaceOneAsync(i => i.Id == item.Id, item);
    }
    public async Task DeleteAsync(string id)
    {
        await _items.DeleteOneAsync(i => i.Id == id);
    }
}
