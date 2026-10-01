using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Blog.Infrastructure.Persistence.Configurations;

internal class PostMetaConfiguration : EntityModelBuilderBase,
      IEntityTypeConfiguration<Domain.PostMeta>
{
    public void Configure(
        EntityTypeBuilder<Domain.PostMeta> builder)
    {
        ConfigureCommonProperties(builder);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LastModified)
            .IsRequired();

        builder
            .HasIndex(x => new
            {
                x.PostId,
                x.Key
            })
            .IsUnique();

        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.Meta)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(
            "PostMeta",
            Schemas.Blog);
    }
}
