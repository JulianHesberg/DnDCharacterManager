using System;
using CharacterMicroservice.Domain.Models.Entity.Write;

namespace CharacterMicroservice.Application.Interfaces.IRepositories;

public interface ICharacterSkillsRepository
{
    Task<IEnumerable<CharacterSkills>> GetByCharacterIdAsync(int characterId);
    Task AddAsync(CharacterSkills skill);
    Task Remove(CharacterSkills skill);

}
