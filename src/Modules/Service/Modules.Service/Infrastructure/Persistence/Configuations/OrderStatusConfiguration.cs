using Common.Infrastructure.Persistence;
using Modules.Service.Domain.Enum;

namespace Modules.Service.Infrastructure.Persistence.Configuations;

internal sealed class OrderStatusConfiguration
    : EntityModelBuilderBase, IEntityTypeConfiguration<Domain.OrderStatus>
{
    public void Configure(EntityTypeBuilder<Domain.OrderStatus> builder)
    {
        ConfigureCommonEnumProperties<Domain.OrderStatus, Domain.Enum.OrderStatusCode>(builder);

        builder.ToTable("OrderStatus", Schemas.Service);
    }
}
