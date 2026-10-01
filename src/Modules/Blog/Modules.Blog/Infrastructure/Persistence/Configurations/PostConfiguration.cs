using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Blog.Infrastructure.Persistence.Configurations;

internal sealed class PostConfiguration
    : EntityModelBuilderBase,
      IEntityTypeConfiguration<Domain.Post>
{
    public void Configure(EntityTypeBuilder<Domain.Post> builder)
    {
        ConfigureCommonProperties(builder);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AuthorId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.MetaTitle)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Summary)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.PostStatus)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Property(x => x.PublishedAt)
            .IsRequired(false);

        // Egy slug csak egy bejegyzéshez tartozhat.
        builder.HasIndex(x => x.Slug)
            .IsUnique();

        // Hasznos lesz listázás/szűrés miatt.
        builder.HasIndex(x => x.PostStatus);

        builder.HasIndex(x => x.PublishedAt);

        builder.HasIndex(x => x.AuthorId);

        // Post -> Parent Post
        builder
            .HasOne(x => x.Parent)
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Post -> Meta
        builder
            .HasMany(x => x.Meta)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post -> PostTags
        builder
            .HasMany(x => x.PostTags)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Meta)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.PostTags)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.ToTable("Posts", Schemas.Blog);
    }
}
