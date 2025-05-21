using AutoMapper;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterSkillsCommands;

public record CreateCharacterSkillCommand(CharacterSkills Skill) : IRequest<Unit>;

public class CreateCharacterSkillCommandHandler : IRequestHandler<CreateCharacterSkillCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCharacterSkillCommandHandler(IUnitOfWork uow, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateCharacterSkillCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterSkills.AddAsync(request.Skill);
        await _uow.SaveChangesAsync(cancellationToken);
        
        var readModel = _mapper.Map<CharacterSkills>(request.Skill);
        await _mediator.Publish(
            new CharacterSkillAddedEvent(request.Skill.CharacterId, request.Skill.SkillId),
            cancellationToken);
        return Unit.Value;
    }
}
