namespace Modules.Chat.Infrastructure.Persistence.Configurations;

internal sealed class AnswerConfiguration
    : EntityModelBuilderBase, IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        ConfigureCommonProperties(builder);
        builder.ToTable("Answers", Schemas.Chat);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(x => x.Intent)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.IntentId);
    }
}
