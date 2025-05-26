using System;
using CharacterMicroservice.Application.ReadModels;
using MediatR;

namespace CharacterMicroservice.Application.Events;

public class DomainEvents
{
    public record CharacterUpsertedEvent(CharacterRead Character) : INotification;
    public record CharacterRemovedEvent(int CharacterId) : INotification;
    public record CharacterItemAddedEvent(int CharacterId, string ItemId) : INotification;
    public record CharacterItemRemovedEvent(int CharacterId, string ItemId) : INotification;
    public record CharacterSkillAddedEvent(int CharacterId, string SkillId) : INotification;
    public record CharacterSkillRemovedEvent(int CharacterId, string SkillId) : INotification;
    public record CharacterNoteAddedEvent(int CharacterId, int NoteId, string Content) : INotification;
    public record CharacterNoteRemovedEvent(int CharacterId, int NoteId, string Content) : INotification;
}
