using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using CharacterMicroservice.Application.ReadModels;
using MongoDB.Driver;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo;

public class CharacterReadRepository : ICharacterReadRepository
{
    private readonly IMongoCollection<CharacterRead> _collection;

    public CharacterReadRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<CharacterRead>("characters");
    }

    public async Task<List<CharacterRead>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection
    .Find(FilterDefinition<CharacterRead>.Empty)
    .ToListAsync(ct);

    }

    public async Task<CharacterRead> GetCharacterByIdAsync(int characterId, CancellationToken ct = default)
    {
        var filter = Builders<CharacterRead>
            .Filter.Eq(c => c.CharacterId, characterId);
        return await _collection
            .Find(filter)
            .FirstOrDefaultAsync(ct);
    }

    public Task Upsert(CharacterRead character, CancellationToken ct = default)
    {
        var filter = Builders<CharacterRead>
            .Filter.Eq(c => c.CharacterId, character.CharacterId);

        return _collection.ReplaceOneAsync(
            filter: filter,
            replacement: character,
            options: new ReplaceOptions { IsUpsert = true },
            cancellationToken: ct);
    }
    public Task Delete(int characterId, CancellationToken ct = default)
    {
        var filter = Builders<CharacterRead>
            .Filter.Eq(c => c.CharacterId, characterId);
        return _collection.DeleteOneAsync(filter, ct);
    }

}
