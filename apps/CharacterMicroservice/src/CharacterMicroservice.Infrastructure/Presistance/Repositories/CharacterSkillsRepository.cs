using System;
using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Domain.Models.Entity.Write;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CharacterMicroservice.Infrastructure.Presistance.Repositories;

public class CharacterSkillsRepository : ICharacterSkillsRepository
{

    private readonly CharacterDbContext _context;

    public CharacterSkillsRepository(CharacterDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CharacterSkills>> GetByCharacterIdAsync(int characterId)
    {
        try
        {
            return await _context.CharacterSkills
                .Where(cs => cs.CharacterId == characterId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving character skills.", ex);
        }
    }
    public async Task AddAsync(CharacterSkills skill)
    {
        try
        {
            await _context.CharacterSkills.AddAsync(skill);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the character skill.", ex);
        }
    }

    public Task Remove(CharacterSkills skill)
    {
        try
        {
            _context.CharacterSkills.Remove(skill);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while removing the character skill.", ex);
        }

    }
}
