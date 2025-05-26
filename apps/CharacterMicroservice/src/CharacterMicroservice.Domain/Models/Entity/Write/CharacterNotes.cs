using System;

namespace CharacterMicroservice.Domain.Models.Entity.Write;

public class CharacterNotes
{

    public int NoteId { get; set; }
    public int CharacterId { get; set; }
    public string Content { get; set; } = string.Empty;

}
