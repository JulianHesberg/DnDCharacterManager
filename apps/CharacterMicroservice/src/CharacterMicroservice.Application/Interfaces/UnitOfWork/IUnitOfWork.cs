using System;
using CharacterMicroservice.Application.Interfaces.IRepositories;

namespace CharacterMicroservice.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    ICharacterSheetRepository Characters { get; }
    ICharacterItemsRepository CharacterItems { get; }
    ICharacterSkillsRepository CharacterSkills { get; }
    ICharacterNotesRepository CharacterNotes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
