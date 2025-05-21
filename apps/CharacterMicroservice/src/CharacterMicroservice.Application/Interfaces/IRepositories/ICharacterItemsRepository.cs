using System;
using CharacterMicroservice.Domain.Models.Entity.Write;

namespace CharacterMicroservice.Application.Interfaces.IRepositories;

public interface ICharacterItemsRepository
{
    Task<IEnumerable<CharacterItems>> GetByCharacterIdAsync(int characterId);
    Task AddAsync(CharacterItems item);
    Task Remove(CharacterItems item);
}
