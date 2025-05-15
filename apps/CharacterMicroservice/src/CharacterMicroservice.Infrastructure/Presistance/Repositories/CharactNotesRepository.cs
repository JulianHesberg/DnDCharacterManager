
using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Domain.Models.Entity.Write;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CharacterMicroservice.Infrastructure.Presistance.Repositories;

public class CharactNotesRepository : ICharacterNotesRepository
{
    private readonly CharacterDbContext _context;

    public CharactNotesRepository(CharacterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CharacterNotes>> GetByCharacterIdAsync(int characterId)
    {
        try
        {
            var result = await _context.CharacterNotes
                .Where(cn => cn.CharacterId == characterId)
                .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the character notes.", ex);
        }
    }

    public async Task AddAsync(CharacterNotes note)
    {
        try
        {
            await _context.CharacterNotes.AddAsync(note);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the character note.", ex);
        }
    }

    public Task Remove(CharacterNotes note)
    {
        try
        {
            _context.CharacterNotes.Remove(note);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while removing the character note.", ex);
        }
    }
}
