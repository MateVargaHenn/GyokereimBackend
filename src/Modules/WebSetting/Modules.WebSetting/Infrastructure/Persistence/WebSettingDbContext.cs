namespace Modules.WebSetting.Infrastructure.Persistence;

internal class WebSettingDbContext(
    DbContextOptions<WebSettingDbContext> options) : DbContext(options)
{

    internal DbSet<Domain.WebSetting> WebSettings => Set<Domain.WebSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
    modelBuilder
                .ApplyConfigurationsFromAssembly(
                typeof(WebSettingDbContext).Assembly);
}
