using System;
using CharacterMicroservice.Domain.Models.Entity.Read;

namespace CharacterMicroservice.Application.Interfaces.IRepositories;

public interface ICharacterReadRepository
{

    Task<CharacterRead> GetCharacterByIdAsync(int characterId);
    Task<List<CharacterRead>> GetAllAsync();

}
