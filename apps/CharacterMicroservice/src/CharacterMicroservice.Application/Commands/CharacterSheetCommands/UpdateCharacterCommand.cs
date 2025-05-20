using AutoMapper;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Application.ReadModels;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterSheetCommands;

public record UpdateCharacterCommand(CharacterSheet Character) : IRequest<Unit>;

public class UpdateCharacterCommandHandler : IRequestHandler<UpdateCharacterCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UpdateCharacterCommandHandler(IUnitOfWork uow, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
    {
        _uow.Characters.Update(request.Character);
        await _uow.SaveChangesAsync(cancellationToken);

        var readModel = _mapper.Map<CharacterRead>(request.Character);
        await _mediator.Publish(
            new CharacterUpsertedEvent(readModel),
            cancellationToken);

        return Unit.Value;
    }
}
