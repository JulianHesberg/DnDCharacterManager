using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMicroservice.Application.Dtos;
using SkillMicroservice.Domain.Interfaces;

namespace SkillMicroservice.Application
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _repository;

        public SkillService(ISkillRepository repository)
        {
            _repository = repository;
        }

        public Task<SkillDto> GetSkillByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SkillDto> CreateSkillAsync(CreateSkillDto skill)
        {
            throw new NotImplementedException();
        }

        public Task<SkillDto> UpdateSkillAsync(UpdateSkillDto skill)
        {
            throw new NotImplementedException();
        }

        public Task<SkillDto> DeleteSkillAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
