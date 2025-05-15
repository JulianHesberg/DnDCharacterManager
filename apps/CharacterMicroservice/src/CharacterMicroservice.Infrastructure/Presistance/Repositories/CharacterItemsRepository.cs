using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Domain.Models.Entity.Write;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CharacterMicroservice.Infrastructure.Presistance.Repositories;

public class CharacterItemsRepository : ICharacterItemsRepository
{

    private readonly CharacterDbContext _context;

    public CharacterItemsRepository(CharacterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CharacterItems>> GetByCharacterIdAsync(int characterId)
    {
        try
        {
            var result = await _context.CharacterItems
            .Where(ci => ci.CharacterId == characterId)
            .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the character items.", ex);
        }
    }

    public async Task AddAsync(CharacterItems item)
    {
        try
        {
            await _context.CharacterItems.AddAsync(item);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the character item.", ex);
        }
    }

    public Task Remove(CharacterItems item)
    {
        try
        {
            _context.CharacterItems.Remove(item);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while removing the character item.", ex);
        }

    }
}
