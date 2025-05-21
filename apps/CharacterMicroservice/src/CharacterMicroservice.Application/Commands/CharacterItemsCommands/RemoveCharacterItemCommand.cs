using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterItemsCommands;

public record RemoveCharacterItemCommand(CharacterItems Item) : IRequest<Unit>;

public class RemoveCharacterItemCommandHandler : IRequestHandler<RemoveCharacterItemCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;

    public RemoveCharacterItemCommandHandler(IUnitOfWork uow, IMediator mediator)
    {
        _mediator = mediator;
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterItemCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterItems.Remove(request.Item);
        await _uow.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(
            new CharacterItemRemovedEvent(request.Item.CharacterId, request.Item.ItemId),
            cancellationToken);
            
        return Unit.Value;
    }
}