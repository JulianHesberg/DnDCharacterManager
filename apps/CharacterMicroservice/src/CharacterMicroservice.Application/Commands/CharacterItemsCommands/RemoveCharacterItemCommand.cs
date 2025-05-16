using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterItemsCommands;

public record RemoveCharacterItemCommand(CharacterItems Item) : IRequest<Unit>;

public class RemoveCharacterItemCommandHandler : IRequestHandler<RemoveCharacterItemCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoveCharacterItemCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterItemCommand request, CancellationToken cancellationToken)
    {
        _uow.CharacterItems.Remove(request.Item);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}