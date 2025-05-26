namespace CharacterMicroservice.Infrastructure.Configurations;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string CharacterCollectionName { get; set; } = string.Empty;
    public string CharacterNotesCollectionName { get; set; } = string.Empty;
    public string CharacterItemsCollectionName { get; set; } = string.Empty;
    public string CharacterSkillsCollectionName { get; set; } = string.Empty;
}