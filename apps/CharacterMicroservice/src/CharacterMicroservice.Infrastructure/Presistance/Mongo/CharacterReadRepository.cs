using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using CharacterMicroservice.Domain.Models.Entity.Read;
using MongoDB.Driver;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo;

public class CharacterReadRepository : ICharacterReadRepository
{
    private readonly IMongoCollection<CharacterRead> _collection;

    public CharacterReadRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<CharacterRead>("characters");
    }

    public async Task<List<CharacterRead>> GetAllAsync()
    {
        return await _collection.Find(Builders<CharacterRead>.Filter.Empty).ToListAsync();
    }

    public async Task<CharacterRead> GetCharacterByIdAsync(int characterId)
    {
        var filter = Builders<CharacterRead>.Filter.Eq(c => c.CharacterId, characterId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpsertAsync(CharacterRead character)
    {
        var filter = Builders<CharacterRead>.Filter.Eq(c => c.CharacterId, character.CharacterId);
        await _collection.ReplaceOneAsync(
            filter,
            character,
            new ReplaceOptions { IsUpsert = true });
    }
    public async Task DeleteAsync(int characterId)
    {
        var filter = Builders<CharacterRead>.Filter.Eq(c => c.CharacterId, characterId);
        await _collection.DeleteOneAsync(filter);
    }

}
