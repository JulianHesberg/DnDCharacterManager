using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using MongoDB.Driver;
using CharacterMicroservice.Infrastructure.Configurations;
using CharacterMicroservice.Infrastructure.Presistance.Mongo;
using CharacterMicroservice.Domain.Models.Entity.Write;
using CharacterMicroservice.Domain.Models.Entity.Enums;

[MemoryDiagnoser]
public class MongoCrudBenchmarks
{
    private IMongoCollection<CharacterSheet> _col;
    private static int _nextId;

    [GlobalSetup]
    public void Setup()
    {
        var settings = new MongoDbSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "BenchmarkDb",
            CharacterCollectionName = "CharacterSheets"
        };
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _col = db.GetCollection<CharacterSheet>(settings.CharacterCollectionName);
        db.DropCollection(settings.CharacterCollectionName);
    }

    [Benchmark]
    public async Task CreateCharacter()
    {
        try
        {
            var id = Interlocked.Increment(ref _nextId);
            var character = new CharacterSheet
            {
                Id = id,
                Name = "Bench_" + id,
                Race = Enum.Parse<Race>("1"),
                Gender = Enum.Parse<Gender>("0"),
                Class = Enum.Parse<Class>("2"),
                Gold = 150,
                Level = 5,
                SkillPoints = 10,
                Health = 100,
                Items = new List<CharacterItems> {
                    new() { ItemId = "sword",   CharacterId = id },
                    new() { ItemId = "shield",  CharacterId = id }
                },
                Skills = new List<CharacterSkills> {
                    new() { SkillId = "tracking",   CharacterId = id },
                    new() { SkillId = "leadership", CharacterId = id }
                },
                Notes = new List<CharacterNotes> {
                    new() { NoteId = 1, Content = "Born in Rivendell.",         CharacterId = id },
                    new() { NoteId = 2, Content = "Wields Andúril, the Sword.", CharacterId = id }
                }
            };

            await _col.InsertOneAsync(character);
        }
        catch (Exception e)
        {
            Console.WriteLine($" An exception was cought on mongo Write: {e.Message}");
        }

    }

    [Benchmark]
    public async Task ReadAllCharacters()
    {
        var all = await _col.Find(_ => true).ToListAsync();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _col.Database.DropCollection(_col.CollectionNamespace.CollectionName);
    }
}
