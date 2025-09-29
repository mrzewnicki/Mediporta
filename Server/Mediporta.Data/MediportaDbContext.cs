using Mediporta.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mediporta.Data;

public class MediportaDbContext:DbContext
{
    public MediportaDbContext(DbContextOptions<MediportaDbContext> options) : base(options) { }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Collective> Collectives { get; set; }
    public DbSet<TagCollective> TagCollectives { get; set; }
    public DbSet<ExternalLink> ExternalLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TagCollective>()
            .HasKey(tc => new { tc.TagId, tc.CollectiveId });

        modelBuilder.Entity<TagCollective>()
            .HasOne(tc => tc.Tag)
            .WithMany(t => t.TagCollectives)
            .HasForeignKey(tc => tc.TagId);

        modelBuilder.Entity<TagCollective>()
            .HasOne(tc => tc.Collective)
            .WithMany(c => c.TagCollectives)
            .HasForeignKey(tc => tc.CollectiveId);
    }
}