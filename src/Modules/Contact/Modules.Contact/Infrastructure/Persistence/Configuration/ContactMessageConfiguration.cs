namespace Modules.Contact.Infrastructure.Persistence.Configuration;

internal sealed class ContactMessageConfiguration
    : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.ToTable(
            "ContactMessages", 
            Schemas.Contact);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(5000)
            .IsRequired();

        builder.Property(x => x.AttachmentId);

        builder.Property(x => x.NotificationSent)
            .IsRequired();

        builder
            .HasOne(x => x.Parent)
            .WithMany()
            .HasForeignKey(x => x.ReplyToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
