using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMicroservice.Domain.Entities;
using SkillMicroservice.Domain.Interfaces;

namespace SkillMicroservice.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Skill> CreateSkillAsync(Skill skill)
        {
            throw new NotImplementedException();
        }

        public async Task<Skill> UpdateSkillAsync(Skill skill)
        {
            throw new NotImplementedException();
        }

        public async Task<Skill> DeleteSkillAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Skill>> GetSkillsByLevelAsync(int level)
        {
            throw new NotImplementedException();
        }
    }
}
