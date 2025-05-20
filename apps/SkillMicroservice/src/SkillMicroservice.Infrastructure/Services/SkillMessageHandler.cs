using MessageBroker.Interfaces;
using SkillMicroservice.Domain.Interfaces;

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

    public Task HandleMessageAsync<T>(T message)
    {
        throw new NotImplementedException();
    }
}