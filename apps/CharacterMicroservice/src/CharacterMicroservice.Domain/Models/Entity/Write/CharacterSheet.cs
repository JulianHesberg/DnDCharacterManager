using System;
using System.Dynamic;
using CharacterMicroservice.Domain.Models.Entity.Enums;

namespace CharacterMicroservice.Domain.Models.Entity.Write;

public class CharacterSheet
{

    public int Id { get; set; }
    public string Name { get; set; }
    public Race Race { get; set; }
    public Gender Gender { get; set; }
    public Class Class { get; set; }
    public int Gold { get; set; }
    public int Level { get; set; }
    public int SkillPoints { get; set; }
    public int Health { get; set; }

    public ICollection<CharacterItems> Items { get; set; }
    public ICollection<CharacterSkills> Skills { get; set; }
    public ICollection<CharacterNotes> Notes { get; set; }
    public int CharacterId { get; set; }
}
