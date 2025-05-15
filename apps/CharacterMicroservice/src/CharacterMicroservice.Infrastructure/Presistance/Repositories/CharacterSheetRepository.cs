using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Domain.Models.Entity.Write;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CharacterMicroservice.Infrastructure.Presistance.Repositories;

public class CharacterSheetRepository : ICharacterSheetRepository
{

    private readonly CharacterDbContext _context;

    public CharacterSheetRepository(CharacterDbContext context)
    {
        _context = context;
    }

    public async Task<CharacterSheet> GetCharacterByIdAsync(int id)
    {
        try
        {
            var result = await _context.CharacterSheets
                .Include(c => c.Items)
                .Include(c => c.Skills)
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the character sheet.", ex);
        }

    }
    public async Task AddAsync(CharacterSheet character)
    {
        try
        {
            await _context.CharacterSheets.AddAsync(character);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the character sheet.", ex);
        }
    }

    public Task UpdateAsync(CharacterSheet character)
    {
        try
        {
            _context.CharacterSheets.Update(character);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the character sheet.", ex);
        }
    }

    public Task DeleteAsync(CharacterSheet character)
    {
        try
        {
            _context.CharacterSheets.Remove(character);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the character sheet.", ex);
        }
    }

}
