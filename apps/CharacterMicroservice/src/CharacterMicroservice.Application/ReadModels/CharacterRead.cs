using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CharacterMicroservice.Application.ReadModels;

public class CharacterRead
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int CharacterId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public int Gold { get; set; }
    public int Level { get; set; }
    public int SkillPoints { get; set; }
    public int Health { get; set; }
    public List<string> Items { get; set; }
    public List<string> Skills { get; set; }
    public List<string> Notes { get; set; }

}
