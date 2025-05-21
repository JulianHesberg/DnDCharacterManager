using System;
using CharacterMicroservice.Application.ReadModels;

namespace CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;

public interface ICharacterReadRepository
{

    Task<CharacterRead> GetCharacterByIdAsync(int characterId, CancellationToken ct);
    Task<List<CharacterRead>> GetAllAsync(CancellationToken ct);

    // Upsert or insert a read model document
    Task Upsert(CharacterRead character, CancellationToken ct);
    // Delete a read model document
    Task Delete(int characterId, CancellationToken ct);
}
