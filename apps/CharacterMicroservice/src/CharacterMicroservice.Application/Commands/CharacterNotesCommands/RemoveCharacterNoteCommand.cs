using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterNotesCommands;

public record RemoveCharacterNoteCommand(CharacterNotes Note) : IRequest<Unit>;

public class RemoveCharacterNoteCommandHandler : IRequestHandler<RemoveCharacterNoteCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;

    public RemoveCharacterNoteCommandHandler(IUnitOfWork uow, IMediator mediator)
    {
        _mediator = mediator;
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterNoteCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterNotes.Remove(request.Note);
        await _uow.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(
            new CharacterNoteRemovedEvent(request.Note.CharacterId, request.Note.NoteId, request.Note.Content),
            cancellationToken);
            
        return Unit.Value;
    }
}   