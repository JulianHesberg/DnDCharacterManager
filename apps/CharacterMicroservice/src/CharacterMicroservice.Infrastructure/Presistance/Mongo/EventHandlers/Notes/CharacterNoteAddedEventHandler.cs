using CharacterMicroservice.Application.Events;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Notes;

public class CharacterNoteAddedEventHandler : INotificationHandler<CharacterNoteAddedEvent>
{
    private readonly ICharacterReadRepository _readRepo;

    public CharacterNoteAddedEventHandler(ICharacterReadRepository readRepo)
        => _readRepo = readRepo;

    public async Task Handle(CharacterNoteAddedEvent notification, CancellationToken cancellationToken)
    {
        var model = await _readRepo.GetCharacterByIdAsync(notification.CharacterId, cancellationToken);
        model.Notes.Add(notification.Content);
        await _readRepo.Upsert(model, cancellationToken);
    }
}

