using System;
using CharacterMicroservice.Domain.Models.Entity.Read;
using MediatR;

namespace CharacterMicroservice.Application.Events;

public class DomainEvents
{
    public record CharacterUpsertedEvent(CharacterRead Character) : INotification;
    public record CharacterDeletedEvent(int CharacterId) : INotification;
    public record CharacterItemAddedEvent(int CharacterId, string ItemId) : INotification;
    public record CharacterItemRemovedEvent(int CharacterId, string ItemId) : INotification;
    public record CharacterSkillAddedEvent(int CharacterId, string SkillId) : INotification;
    public record CharacterSkillRemovedEvent(int CharacterId, string SkillId) : INotification;
    public record SkillNoteAddedEvent(int CharacterId, int NoteId, string Content) : INotification;
    public record SkillNoteRemovedEvent(int CharacterId, int NoteId) : INotification;
}
