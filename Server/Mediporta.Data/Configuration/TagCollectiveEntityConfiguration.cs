using Mediporta.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mediporta.Data.Configuration;

public class TagCollectiveEntityConfiguration : IEntityTypeConfiguration<TagCollective>
{
    public void Configure(EntityTypeBuilder<TagCollective> builder)
    {
        builder.HasKey(tc => new { tc.TagId, tc.CollectiveId });

        builder.HasOne(tc => tc.Tag)
            .WithMany(t => t.TagCollectives)
            .HasForeignKey(tc => tc.TagId);

        builder.HasOne(tc => tc.Collective)
            .WithMany(c => c.TagCollectives)
            .HasForeignKey(tc => tc.CollectiveId);
    }
}