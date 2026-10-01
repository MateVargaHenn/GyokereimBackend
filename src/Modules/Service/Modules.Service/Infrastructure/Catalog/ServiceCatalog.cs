using Modules.Service.Application.Abstractions;
using Modules.Service.Contracts;

namespace Modules.Service.Infrastructure.Catalog;

internal sealed class ServiceCatalog(ServiceDbContext context) : IServiceCatalog
{
    public async Task<IReadOnlyCollection<ServicePriceDto>> GetServicesAsync(CancellationToken cancellationToken = default) => await context.Services
            .AsNoTracking()
            .Select(service => new ServicePriceDto(
                service.Id,
                service.Slug,
                service.Name,
                service.ShortDescription,
                service.Description,
                service.Price))
            .ToListAsync(cancellationToken);
}
