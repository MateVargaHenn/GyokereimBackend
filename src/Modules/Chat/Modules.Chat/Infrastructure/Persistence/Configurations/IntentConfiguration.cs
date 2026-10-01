namespace Modules.Chat.Infrastructure.Persistence.Configurations;

internal sealed class IntentConfiguration
    : IEntityTypeConfiguration<Intent>
{
    public void Configure(EntityTypeBuilder<Intent> builder)
    {
        builder.ToTable("Intents", Schemas.Chat);

        builder.HasKey(x => x.Id);
        builder
            .HasIndex(x => x.Key)
            .IsUnique();
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
