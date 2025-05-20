using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Character;

public class CharacterUpsertedEventHandler : INotificationHandler<CharacterUpsertedEvent>
{

    private readonly ICharacterReadRepository _readRepository;

    public CharacterUpsertedEventHandler(ICharacterReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task Handle(CharacterUpsertedEvent notification, CancellationToken cancellationToken)
        => _readRepository.Upsert(notification.Character, cancellationToken);
}

