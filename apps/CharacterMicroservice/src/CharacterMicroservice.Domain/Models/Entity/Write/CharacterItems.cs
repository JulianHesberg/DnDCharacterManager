using System;

namespace CharacterMicroservice.Domain.Models.Entity.Write;

public class CharacterItems
{
    public string ItemId { get; set; } = string.Empty;
    public int CharacterId { get; set; }
    
}
