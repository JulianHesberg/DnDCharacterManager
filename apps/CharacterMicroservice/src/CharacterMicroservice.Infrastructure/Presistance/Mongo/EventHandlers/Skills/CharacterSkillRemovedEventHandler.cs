using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Skills;

public class CharacterSkillRemovedEventHandler : INotificationHandler<CharacterSkillRemovedEvent>
{

    private readonly ICharacterReadRepository _readRepository;

    public CharacterSkillRemovedEventHandler(ICharacterReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task Handle(CharacterSkillRemovedEvent notification, CancellationToken cancellationToken)
    {
            var model = await _readRepository.GetCharacterByIdAsync(notification.CharacterId, cancellationToken);
            model.Skills.Remove(notification.SkillId);
            await _readRepository.Upsert(model, cancellationToken);
    }

}
