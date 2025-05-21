using System;

namespace CharacterMicroservice.Domain.Models.Entity.Write;

public class CharacterSkills
{
    public string SkillId { get; set; } = string.Empty;
    public int CharacterId { get; set; }

}
