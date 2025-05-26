using System;
using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using CharacterMicroservice.Infrastructure.Presistance.Repositories;

namespace CharacterMicroservice.Infrastructure.Presistance;

public class UnitOfWork : IUnitOfWork
{

    private readonly CharacterDbContext _context;
    public ICharacterSheetRepository Characters { get; }
    public ICharacterItemsRepository CharacterItems { get; }
    public ICharacterSkillsRepository CharacterSkills { get; }
    public ICharacterNotesRepository CharacterNotes { get; }

    public UnitOfWork(CharacterDbContext context,
                      ICharacterSheetRepository chars,
                      ICharacterItemsRepository items,
                      ICharacterSkillsRepository skills,
                      ICharacterNotesRepository notes)
    {
        _context = context;
        Characters = chars;
        CharacterItems = items;
        CharacterSkills = skills;
        CharacterNotes = notes;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

}
