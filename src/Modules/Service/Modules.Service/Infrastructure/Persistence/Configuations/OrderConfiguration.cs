

namespace Modules.Service.Infrastructure.Persistence.Configuations;

internal sealed class OrderConfiguration
    : EntityModelBuilderBase,
      IEntityTypeConfiguration<Domain.Order>
{
    public void Configure(
        EntityTypeBuilder<Domain.Order> builder)
    {
        ConfigureCommonProperties(builder);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.CustomerName)
            .IsRequired();

        builder
            .Property(x => x.Email)
            .IsRequired();

        builder
            .Property(x => x.PhoneNumber)
            .IsRequired();

        builder
            .Property(x => x.Message);

        builder
            .Property(x => x.TrackingCode)
            .IsRequired();

        builder
            .Property(x => x.Created)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder
            .Property(x => x.OrderStatusCode)
            .IsRequired();

        builder
            .Property(x => x.ServiceId)
            .IsRequired();

        builder
            .HasOne(x => x.Service)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.ServiceId);

        builder
            .HasIndex(x => x.TrackingCode)
            .IsUnique();

        builder
            .HasOne(x => x.OrderStatus)
            .WithMany()
            .HasForeignKey(x => x.OrderStatusCode);

        builder.ToTable(
            "Orders",
            Schemas.Service);
    }
}
