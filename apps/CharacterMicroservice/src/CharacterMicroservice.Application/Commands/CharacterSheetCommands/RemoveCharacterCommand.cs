using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterSheetCommands;

public record RemoveCharacterCommand(int Id) : IRequest<Unit>;

public class RemoveCharacterCommandHandler : IRequestHandler<RemoveCharacterCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;

    public RemoveCharacterCommandHandler(IUnitOfWork uow, IMediator mediator)
    {
        _mediator = mediator;
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterCommand request, CancellationToken cancellationToken)
    {
        var character = await _uow.Characters.GetCharacterByIdAsync(request.Id);
        if (character != null)
            _uow.Characters.Remove(character);

        await _uow.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(
            new CharacterRemovedEvent(request.Id),
            cancellationToken);

        return Unit.Value;
    }
}

