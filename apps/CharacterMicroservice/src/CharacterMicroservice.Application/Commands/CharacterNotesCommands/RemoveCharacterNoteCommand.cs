using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterNotesCommands;

public record RemoveCharacterNoteCommand(CharacterNotes Note) : IRequest<Unit>;

public class RemoveCharacterNoteCommandHandler : IRequestHandler<RemoveCharacterNoteCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoveCharacterNoteCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterNoteCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterNotes.Remove(request.Note);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}