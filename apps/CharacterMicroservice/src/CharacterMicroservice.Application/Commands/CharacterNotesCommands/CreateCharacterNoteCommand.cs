using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterNotesCommands;

public record CreateCharacterNoteCommand(CharacterNotes Note) : IRequest<Unit>;

public class CreateCharacterNoteCommandHandler : IRequestHandler<CreateCharacterNoteCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public CreateCharacterNoteCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateCharacterNoteCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterNotes.AddAsync(request.Note);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
