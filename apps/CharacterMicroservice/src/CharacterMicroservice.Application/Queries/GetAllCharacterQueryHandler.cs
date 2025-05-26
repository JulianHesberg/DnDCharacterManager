using System;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using CharacterMicroservice.Application.ReadModels;
using MediatR;

namespace CharacterMicroservice.Application.Queries;

// Query: Get all characters
public record GetAllCharactersQuery() : IRequest<IEnumerable<CharacterRead>>;

public class GetAllCharactersQueryHandler : IRequestHandler<GetAllCharactersQuery, IEnumerable<CharacterRead>>
{
    private readonly ICharacterReadRepository _repository;

    public GetAllCharactersQueryHandler(ICharacterReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CharacterRead>> Handle(
        GetAllCharactersQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
