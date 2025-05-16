using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterItemsCommands;

// 4. Add Item
public record CreateCharacterItemCommand(CharacterItems Item) : IRequest<Unit>;

public class CreateCharacterItemCommandHandler : IRequestHandler<CreateCharacterItemCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public CreateCharacterItemCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateCharacterItemCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterItems.AddAsync(request.Item);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}