using AutoMapper;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterItemsCommands;

// 4. Add Item
public record CreateCharacterItemCommand(CharacterItems Item) : IRequest<Unit>;

public class CreateCharacterItemCommandHandler : IRequestHandler<CreateCharacterItemCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCharacterItemCommandHandler(IUnitOfWork uow, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateCharacterItemCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterItems.AddAsync(request.Item);
        await _uow.SaveChangesAsync(cancellationToken);

        var readModel = _mapper.Map<CharacterItems>(request.Item);
        await _mediator.Publish(
            new CharacterItemAddedEvent(request.Item.CharacterId, request.Item.ItemId),
            cancellationToken);

        return Unit.Value;
    }
}