using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Notes;

public class CharacterNoteRemovedEventHandler : INotificationHandler<CharacterNoteRemovedEvent>
{
    private readonly ICharacterReadRepository _readRepository;

    public CharacterNoteRemovedEventHandler(ICharacterReadRepository readRepository)
        => _readRepository = readRepository;

    public async Task Handle(CharacterNoteRemovedEvent notification, CancellationToken cancellationToken)
    {
        var model = await _readRepository.GetCharacterByIdAsync(notification.CharacterId, cancellationToken);
        model.Notes.RemoveAll(n => n == notification.Content);
        await _readRepository.Upsert(model, cancellationToken);
    }
}
