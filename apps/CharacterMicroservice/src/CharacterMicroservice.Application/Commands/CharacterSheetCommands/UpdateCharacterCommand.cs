using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterSheetCommands;

public record UpdateCharacterCommand(CharacterSheet Character) : IRequest<Unit>;

public class UpdateCharacterCommandHandler : IRequestHandler<UpdateCharacterCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public UpdateCharacterCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
    {
        _uow.Characters.Update(request.Character);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
