namespace Modules.File.Infrastructure.Persistence.Configurations;


internal sealed class FileConfiguration
    : EntityModelBuilderBase,
      IEntityTypeConfiguration<Domain.File>
{
    public void Configure(
        EntityTypeBuilder<Domain.File> builder)
    {
        ConfigureCommonProperties(builder);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Key)
            .IsRequired();

        builder
            .HasIndex(x => x.Key)
            .IsUnique();

        builder
            .Property(x => x.FileName)
            .IsRequired();

        builder
            .Property(x => x.ContentType)
            .IsRequired();

        builder
            .Property(x => x.Size)
            .IsRequired();

        builder
            .Property(x => x.StoragePath)
            .IsRequired();

        builder
            .Property(x => x.Title);

        builder
            .Property(x => x.AltText);

        builder
            .Property(x => x.IsPublic)
            .HasDefaultValue(false)
            .IsRequired();
    }
}
