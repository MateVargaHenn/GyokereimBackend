namespace Modules.Service.Infrastructure.Persistence.Configuations;

internal sealed class ServiceStatusConfiguration
    : EntityModelBuilderBase, IEntityTypeConfiguration<Domain.ServiceStatus>
{
    public void Configure(EntityTypeBuilder<Domain.ServiceStatus> builder)
    {
        ConfigureCommonEnumProperties<Domain.ServiceStatus, Domain.Enum.ServiceStatusCode>(builder);

        builder.ToTable("ServiceStatus", Schemas.Service);
    }
}
