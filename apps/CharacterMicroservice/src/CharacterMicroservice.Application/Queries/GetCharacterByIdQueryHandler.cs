using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using CharacterMicroservice.Application.ReadModels;
using MediatR;

namespace CharacterMicroservice.Application.Queries;

// Query: Get a single character by ID
public record GetCharacterByIdQuery(int CharacterId) : IRequest<CharacterRead>;

public class GetCharacterByIdQueryHandler : IRequestHandler<GetCharacterByIdQuery, CharacterRead>
{
    private readonly ICharacterReadRepository _repository;

    public GetCharacterByIdQueryHandler(ICharacterReadRepository repository, CancellationToken cancellationToken)
    {
        _repository = repository;
    }

    public async Task<CharacterRead> Handle(
        GetCharacterByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetCharacterByIdAsync(request.CharacterId, cancellationToken);
    }
}
