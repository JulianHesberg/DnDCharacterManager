using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Items;

public class CharacterItemRemovedEventHandler : INotificationHandler<CharacterItemRemovedEvent>
{
    private readonly ICharacterReadRepository _readRepository;

    public CharacterItemRemovedEventHandler(ICharacterReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task Handle(CharacterItemRemovedEvent notification, CancellationToken cancellationToken)
    {
        var model = await _readRepository.GetCharacterByIdAsync(notification.CharacterId, cancellationToken);
        model.Items.Remove(notification.ItemId);
        await _readRepository.Upsert(model, cancellationToken);
    }
}
