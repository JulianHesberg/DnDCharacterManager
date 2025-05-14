using SkillMicroservice.Domain.Entities;

namespace SkillMicroservice.Domain.Interfaces;

public interface ISkillRepository
{
    public Task<Skill> GetSkillByIdAsync(int id);
    public  Task<IEnumerable<Skill>> GetAllSkillsAsync();
    public  Task<Skill> AddSkillAsync(Skill skill);
    public  Task<Skill> UpdateSkillAsync(Skill skill);
    public  Task<bool> DeleteSkillAsync(int id);
}