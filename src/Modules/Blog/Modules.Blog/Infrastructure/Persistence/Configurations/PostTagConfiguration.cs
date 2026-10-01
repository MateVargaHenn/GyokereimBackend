using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Blog.Infrastructure.Persistence.Configurations;

internal sealed class PostTagConfiguration
    : EntityModelBuilderBase,
      IEntityTypeConfiguration<Domain.PostTag>
{
    public void Configure(
        EntityTypeBuilder<Domain.PostTag> builder)
    {
        ConfigureCommonProperties(builder);

        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => new
            {
                x.PostId,
                x.TagId
            })
            .IsUnique();

        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.PostTags)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Tag)
            .WithMany(x => x.PostTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(
            "PostTags",
            Schemas.Blog);
    }
}
