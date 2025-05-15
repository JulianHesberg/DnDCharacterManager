using CharacterMicroservice.Domain.Models.Entity.Write;

namespace CharacterMicroservice.Application.Interfaces.IRepositories;

public interface ICharacterSheetRepository
{

    Task<CharacterSheet> GetCharacterByIdAsync(int id);
    Task AddAsync(CharacterSheet character);
    Task UpdateAsync(CharacterSheet character);
    Task DeleteAsync(CharacterSheet character);

}
