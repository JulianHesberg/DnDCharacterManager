using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Character;

public class CharacterDeletedEventHandler : INotificationHandler<CharacterRemovedEvent>
{

    private readonly ICharacterReadRepository _readRepository;

    public CharacterDeletedEventHandler(ICharacterReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task Handle(CharacterRemovedEvent notification, CancellationToken cancellationToken)
        => _readRepository.Delete(notification.CharacterId, cancellationToken);

}
