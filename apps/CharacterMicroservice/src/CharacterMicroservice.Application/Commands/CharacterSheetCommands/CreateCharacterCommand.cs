using AutoMapper;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Application.ReadModels;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterSheetCommands;

// 1. Create Character
public record CreateCharacterCommand(CharacterSheet Character) : IRequest<int>;

public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, int>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCharacterCommandHandler(IUnitOfWork uow, IMediator mediator, IMapper mapper)
    {
        _uow = uow;
        _mediator = mediator;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
    {
        // 1) Persist to write DB
        await _uow.Characters.AddAsync(request.Character);
        await _uow.SaveChangesAsync(cancellationToken);

        // 2) Map domain entity to read model using AutoMapper for separation of concerns
        var readModel = _mapper.Map<CharacterRead>(request.Character);

        // 3) Publish event and await read-model update handlers
        await _mediator.Publish(
            new CharacterUpsertedEvent(readModel),
            cancellationToken);

        // 4) Return new ID
        return request.Character.CharacterId;
    }
}