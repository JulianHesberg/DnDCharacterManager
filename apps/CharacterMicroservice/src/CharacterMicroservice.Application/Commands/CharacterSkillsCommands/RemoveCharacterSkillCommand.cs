using AutoMapper;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterSkillsCommands;

public record RemoveCharacterSkillCommand(CharacterSkills Skill) : IRequest<Unit>;

public class RemoveCharacterSkillCommandHandler : IRequestHandler<RemoveCharacterSkillCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RemoveCharacterSkillCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterSkillCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterSkills.Remove(request.Skill);
        await _uow.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(
            new CharacterSkillRemovedEvent(request.Skill.CharacterId, request.Skill.SkillId),
            cancellationToken);
            
        return Unit.Value;
    }
}
