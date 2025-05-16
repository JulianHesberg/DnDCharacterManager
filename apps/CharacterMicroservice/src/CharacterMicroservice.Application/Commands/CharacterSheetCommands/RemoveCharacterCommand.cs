using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterSheetCommands;

public record RemoveCharacterCommand(int Id) : IRequest<Unit>;

public class RemoveCharacterCommandHandler : IRequestHandler<RemoveCharacterCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoveCharacterCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveCharacterCommand request, CancellationToken cancellationToken)
    {
        var character = await _uow.Characters.GetCharacterByIdAsync(request.Id);
        if (character != null)
            _uow.Characters.Remove(character);

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

