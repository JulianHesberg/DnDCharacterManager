using System;
using CharacterMicroservice.Domain.Models.Entity.Write;

namespace CharacterMicroservice.Application.Interfaces.IRepositories;

public interface ICharacterNotesRepository
{
    Task<IEnumerable<CharacterNotes>> GetByCharacterIdAsync(int characterId);
    Task AddAsync(CharacterNotes note);
    Task Remove(CharacterNotes note);

}
