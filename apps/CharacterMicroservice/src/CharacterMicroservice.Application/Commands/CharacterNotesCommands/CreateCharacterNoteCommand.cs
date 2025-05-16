using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterNotesCommands;

public record CreateSkillNoteCommand(CharacterNotes Note) : IRequest<Unit>;

public class CreateSkillNoteCommandHandler : IRequestHandler<CreateSkillNoteCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public CreateSkillNoteCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateSkillNoteCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterNotes.AddAsync(request.Note);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
