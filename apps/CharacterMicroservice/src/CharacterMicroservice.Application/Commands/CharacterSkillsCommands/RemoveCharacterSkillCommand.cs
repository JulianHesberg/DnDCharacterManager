using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterSkillsCommands;

public record RemoveCharacterSkillCommand(CharacterSkills Skill) : IRequest<Unit>;

public class RemoveCharacterSkillCommandHandler : IRequestHandler<RemoveCharacterSkillCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoveCharacterSkillCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterSkillCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterSkills.Remove(request.Skill);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
