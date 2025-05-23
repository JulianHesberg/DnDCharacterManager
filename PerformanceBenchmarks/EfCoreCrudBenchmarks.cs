using BenchmarkDotNet.Attributes;
using CharacterMicroservice.Domain.Models.Entity.Enums;
using CharacterMicroservice.Domain.Models.Entity.Write;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace PerformanceBenchmarks;

public class EfCoreCrudBenchmarks
{

    private CharacterDbContext _context;

    [GlobalSetup]
    public void Setup()
    {
        var opts = new DbContextOptionsBuilder<CharacterDbContext>()
            .UseSqlServer("Server=localhost,1433;Database=CharacterDb;User Id=sa;Password=Your_strong_P@ssw0rd;TrustServerCertificate=true;")
            .Options;

        _context = new CharacterDbContext(opts);
        _context.Database.EnsureCreated();
    }

    [Benchmark]
    public async Task CreateCharacter()
    {
        var character = new CharacterSheet
        {
            // Id left off or zero
            Name = "BenchmarkHero",
            Race = Enum.Parse<Race>("3"),
            Gender = Enum.Parse<Gender>("0"),
            Class = Enum.Parse<Class>("2"),
            Gold = 150,
            Level = 5,
            SkillPoints = 10,
            Health = 100,
            Items = new List<CharacterItems> {
            new() { ItemId = "sword" },
            new() { ItemId = "shield" }
        },
            Skills = new List<CharacterSkills> {
            new() { SkillId = "tracking" },
            new() { SkillId = "leadership" }
        },
            Notes = new List<CharacterNotes> {
            new() { Content = "Born in Rivendell, heir of Isildur." },
            new() { Content = "Wields Andúril, the Flame of the West." }
        }
        };
        _context.CharacterSheets.Add(character);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public async Task ReadAllCharacters()
    {
        var all = await _context.CharacterSheets.ToListAsync();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

}
