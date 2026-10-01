using Common.SharedKernel.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.WebSetting.Infrastructure.Persistence.Configurations;

internal sealed class WebSettingConfiguration
    : EntityModelBuilderBase, IEntityTypeConfiguration<Domain.WebSetting>
{
    public void Configure(EntityTypeBuilder<Domain.WebSetting> builder)
    {
        ConfigureCommonProperties(builder);
        builder.ToTable("WebSettings", Schemas.WebSetting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(65000)
            .IsRequired();
    }
}
