using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;

namespace CharacterMicroservice.Application.Commands.CharacterSheetCommands;

// 1. Create Character
public record CreateCharacterCommand(CharacterSheet Character) : IRequest<int>;

public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, int>
{
    private readonly IUnitOfWork _uow;

    public CreateCharacterCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
    {
        await _uow.Characters.AddAsync(request.Character);
        await _uow.SaveChangesAsync(cancellationToken);
        return request.Character.Id;
    }
}