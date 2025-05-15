using System;
using CharacterMicroservice.Domain.Models.Entity.Write;
using Microsoft.EntityFrameworkCore;

namespace CharacterMicroservice.Infrastructure.Presistance.DbContexts;

public class CharacterDbContext : DbContext
{

    public CharacterDbContext(DbContextOptions<CharacterDbContext> options) : base(options) { }

    public DbSet<CharacterSheet> CharacterSheet { get; set; }
    public DbSet<CharacterItems> CharacterItems { get; set; }
    public DbSet<CharacterSkills> CharacterSkills { get; set; }
    public DbSet<CharacterNotes> CharacterNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterSheet>().HasKey(c => c.Id);

        modelBuilder.Entity<CharacterItems>().HasKey(ci => new { ci.ItemId, ci.CharacterId });
        modelBuilder.Entity<CharacterItems>()
            .HasOne<CharacterSheet>()
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CharacterId);

        modelBuilder.Entity<CharacterSkills>().HasKey(cs => new { cs.SkillId, cs.CharacterId });
        modelBuilder.Entity<CharacterSkills>()
            .HasOne<CharacterSheet>()
            .WithMany(c => c.Skills)
            .HasForeignKey(cs => cs.CharacterId);

        modelBuilder.Entity<CharacterNotes>().HasKey(sn => sn.NoteId);
        modelBuilder.Entity<CharacterNotes>()
            .HasOne<CharacterSheet>()
            .WithMany(c => c.Notes)
            .HasForeignKey(sn => sn.CharacterId);
    }
}
