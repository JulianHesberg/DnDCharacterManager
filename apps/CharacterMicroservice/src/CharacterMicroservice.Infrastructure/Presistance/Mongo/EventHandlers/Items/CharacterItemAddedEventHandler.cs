using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Items;

public class CharacterItemAddedEventHandler: INotificationHandler<CharacterItemAddedEvent>
{
    private readonly ICharacterReadRepository _readRepository;

    public CharacterItemAddedEventHandler(ICharacterReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task Handle(CharacterItemAddedEvent notification, CancellationToken cancellationToken)
    {
        var model = await _readRepository.GetCharacterByIdAsync(notification.CharacterId, cancellationToken);
        model.Items.Add(notification.ItemId);
        await _readRepository.Upsert(model, cancellationToken);
    }
}
