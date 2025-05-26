using System;
using System.Linq.Expressions;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Skills;

public class CharacterSkillAddedEventHandler : INotificationHandler<CharacterSkillAddedEvent>
{
    private readonly ICharacterReadRepository _readRepository;

    public CharacterSkillAddedEventHandler(ICharacterReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task Handle(CharacterSkillAddedEvent notification, CancellationToken cancellationToken)
    {
        var model = await _readRepository.GetCharacterByIdAsync(notification.CharacterId, cancellationToken);
        model.Skills.Add(notification.SkillId);
        await _readRepository.Upsert(model, cancellationToken);
    }

}
