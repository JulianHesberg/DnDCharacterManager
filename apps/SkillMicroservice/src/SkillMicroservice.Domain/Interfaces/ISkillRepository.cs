using SkillMicroservice.Domain.Entities;

namespace SkillMicroservice.Domain.Interfaces;

public interface ISkillRepository
{
    public Task<Skill> GetSkillByIdAsync(int id);
    public  Task<IEnumerable<Skill>> GetAllSkillsAsync();
    public  Task<Skill> CreateSkillAsync(Skill skill);
    public  Task<Skill> UpdateSkillAsync(Skill skill);
    public  Task<Skill>DeleteSkillAsync(int id);
    public Task<IEnumerable<Skill>> GetSkillsByLevelAsync(int level);
}