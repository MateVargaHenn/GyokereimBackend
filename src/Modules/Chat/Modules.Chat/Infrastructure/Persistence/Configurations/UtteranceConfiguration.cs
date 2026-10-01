namespace Modules.Chat.Infrastructure.Persistence.Configurations;

internal sealed class UtteranceConfiguration
    : IEntityTypeConfiguration<Utterance>
{
    public void Configure(EntityTypeBuilder<Utterance> builder)
    {
        builder.ToTable("Utterances", Schemas.Chat);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(x => x.Intent)
            .WithMany(x => x.Utterances)
            .HasForeignKey(x => x.IntentId);
    }
}
