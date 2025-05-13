namespace ItemMicroservice.Domain.Entities;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Items
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

}