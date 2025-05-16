using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterSkillsCommands;

public record CreateCharacterSkillCommand(CharacterSkills Skill) : IRequest<Unit>;

public class CreateCharacterSkillCommandHandler : IRequestHandler<CreateCharacterSkillCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public CreateCharacterSkillCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateCharacterSkillCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterSkills.AddAsync(request.Skill);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
