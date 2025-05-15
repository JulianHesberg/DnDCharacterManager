using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMicroservice.Application.Dtos;
using SkillMicroservice.Domain.Entities;
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

        public async Task<SkillDto> GetSkillByIdAsync(int id)
        {

            var skill = await _repository.GetSkillByIdAsync(id);
            return new SkillDto
            {
                Id = skill.Id,
                Description = skill.Description,
                Cost = skill.Cost
            };
           
        }

        public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
        {
            var skill = await _repository.GetAllSkillsAsync();
            return skill.Select(s => new SkillDto
            {
                Id = s.Id,
                Description = s.Description,
                Cost = s.Cost
            });
        }

        public async Task<SkillDto> CreateSkillAsync(CreateSkillDto dto)
        {
            var skill = new Skill
            {
                Description = dto.Description,
                Cost = dto.Cost
            };

            var returnedSkill = await _repository.CreateSkillAsync(skill);
    
            return new SkillDto
            {
                Id = returnedSkill.Id,
                Description = returnedSkill.Description,
                Cost = returnedSkill.Cost
            };
        }

        public async Task<SkillDto> UpdateSkillAsync(UpdateSkillDto dto)
        {
            var skill = new Skill
            {
                Description = dto.Description,
                Cost = dto.Cost
            };

            var returnedSkill = await _repository.UpdateSkillAsync(skill);

            return new SkillDto
            {
                Id = returnedSkill.Id,
                Description = returnedSkill.Description,
                Cost = returnedSkill.Cost
            };
        }
        

        public async Task<SkillDto> DeleteSkillAsync(int id)
        {
            var skill = await _repository.GetSkillByIdAsync(id);
         
            return new SkillDto
            {
                Id = skill.Id,
                Description = skill.Description,
                Cost = skill.Cost
            };
           
        }
    }
}
