using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SkillMicroservice.Application.Interfaces;
using SkillMicroservice.Domain.Entities;
using SkillMicroservice.Infrastructure.Configurations;

namespace SkillMicroservice.Infrastructure.Repositories;

public class ItemsRepository : IItemsRepository
{
    private readonly IMongoCollection<Items> _items;

    public ItemsRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _items = database.GetCollection<Items>(settings.Value.ItemsCollectionName);
    }

    public async Task<List<Items>> GetAllItemsAsync()
    {
        var result = await _items.Find(_ => true).ToListAsync();
        return result;
    }

    public async Task<Items?> GetItemByIdAsync(int id)
    {
        return await _items.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Items item)
    {
        await _items.Find(i => i.Id == item.Id).FirstOrDefaultAsync();
    }
    public async Task UpdateAsync(Items item)
    {
        await _items.ReplaceOneAsync(i => i.Id == item.Id, item);
    }
    public async Task DeleteAsync(int id)
    {
        await _items.DeleteOneAsync(i => i.Id == id);
    }
}
