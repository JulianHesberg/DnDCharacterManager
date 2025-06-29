using MessageBroker.Interfaces;
using MessageBroker.Requests;
using SkillMicroservice.Domain.Interfaces;
using MessageBroker;
using SagaCoordinator.Domain.ResponseModels;
using Skill = SkillMicroservice.Domain.Entities.Skill;

namespace SkillMicroservice.Infrastructure.Services;

public class SkillMessageHandler : IMessageHandler
{

    private readonly IMessageBroker _messageBroker;
    private readonly ISkillRepository _repository;
    public SkillMessageHandler(IMessageBroker messageBroker, ISkillRepository repository)
    {
        _repository = repository;
        _messageBroker = messageBroker;
    }

    public async Task HandleMessageAsync<T>(T message)
    {
        switch (message)
        {
            case LevelUpRequest request:
                await HandleLevelUpRequest(request);
                break;
                
            default:
                throw new NotImplementedException($"Message type {typeof(T).Name} is not implemented.");
        }
    }

    private async Task HandleLevelUpRequest(LevelUpRequest request)
    {
        var skills = await _repository.GetSkillsByLevelAsync(request.Level);

       
        var responseSkills = skills.Select(skill => new SagaCoordinator.Domain.ResponseModels.Skill
        {
            Cost = skill.Cost,
            Description = skill.Description
        }).ToList();

        await _messageBroker.Publish(QueueNames.SkillServiceQueue, new SkillListResponse
        {
            SagaId = request.SagaId,
            CharacterId = request.CharacterId,
            Skills = responseSkills 
        });
    }

    public Task HandleMessageAsync(IMessage message)
    {
        throw new NotImplementedException();
    }
}