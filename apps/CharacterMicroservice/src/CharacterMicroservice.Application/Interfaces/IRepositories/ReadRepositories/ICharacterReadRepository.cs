using System;
using CharacterMicroservice.Domain.Models.Entity.Read;

namespace CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;

public interface ICharacterReadRepository
{

    Task<CharacterRead> GetCharacterByIdAsync(int characterId);
    Task<List<CharacterRead>> GetAllAsync();

    // Upsert or insert a read model document
    Task UpsertAsync(CharacterRead character);
    // Delete a read model document
    Task DeleteAsync(int characterId);

}
