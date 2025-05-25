using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SagaCoordinator.Domain.ResponseModels;
using SkillMicroservice.Domain.Entities;
using SkillMicroservice.Domain.Interfaces;
using SkillMicroservice.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skill = SkillMicroservice.Domain.Entities.Skill;

namespace SkillMicroservice.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {

        private readonly IMongoCollection<Skill> _skills;

        public SkillRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _skills = database.GetCollection<Skill>(settings.Value.SkillsCollectionName);
        }

        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            return await _skills.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            var result = await _skills.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<Skill> CreateSkillAsync(Skill skill)
        {
            await _skills.InsertOneAsync(skill);
            return skill;
        }

        public async Task<Skill> UpdateSkillAsync(Skill skill)
        {
            await _skills.ReplaceOneAsync(s => s.Id == skill.Id, skill);
            return skill;
        }

        public async Task<Skill> DeleteSkillAsync(int id)
        {
            var returnSkill = await GetSkillByIdAsync(id);
            await _skills.DeleteOneAsync(s => s.Id == id);
            return returnSkill;
        }

        public async Task<IEnumerable<Skill>> GetSkillsByLevelAsync(int level)
        {
            return await _skills.Find(s => s.Level == level).ToListAsync();
        }
    }
}
