namespace Modules.Service.Infrastructure.Persistence.Configuations;

internal sealed class ServiceConfiguration
    : EntityModelBuilderBase, IEntityTypeConfiguration<Domain.Service>
{
    public void Configure(EntityTypeBuilder<Domain.Service> builder)
    {
        ConfigureCommonProperties(builder);

        builder.ToTable("Services", Schemas.Service);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.ShortDescription)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.Slug)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(x => x.OrderNumber)
            .IsRequired();

        builder.Property(x => x.ServiceStatusCode)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();
    }
}
