using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterNotesCommands;

public record RemoveSkillNoteCommand(CharacterNotes Note) : IRequest<Unit>;

public class RemoveSkillNoteCommandHandler : IRequestHandler<RemoveSkillNoteCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoveSkillNoteCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveSkillNoteCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterNotes.Remove(request.Note);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}