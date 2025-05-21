using AutoMapper;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using static CharacterMicroservice.Application.Events.DomainEvents;

namespace CharacterMicroservice.Application.Commands.CharacterNotesCommands;

public record CreateCharacterNoteCommand(CharacterNotes Note) : IRequest<Unit>;

public class CreateCharacterNoteCommandHandler : IRequestHandler<CreateCharacterNoteCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCharacterNoteCommandHandler(IUnitOfWork uow, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateCharacterNoteCommand request, CancellationToken cancellationToken)
    {
        await _uow.CharacterNotes.AddAsync(request.Note);
        await _uow.SaveChangesAsync(cancellationToken);

        var readModel = _mapper.Map<CharacterNotes>(request.Note);
        await _mediator.Publish(
            new CharacterNoteAddedEvent(request.Note.CharacterId, request.Note.NoteId, request.Note.Content),
            cancellationToken);
            
        return Unit.Value;
    }
}
