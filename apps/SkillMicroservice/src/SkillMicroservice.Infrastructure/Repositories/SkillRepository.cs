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
        public Task<Skill> GetSkillByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Skill> AddSkillAsync(Skill skill)
        {
            throw new NotImplementedException();
        }

        public Task<Skill> UpdateSkillAsync(Skill skill)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSkillAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
