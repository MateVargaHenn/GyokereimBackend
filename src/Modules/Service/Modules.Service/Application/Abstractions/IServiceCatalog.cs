using Modules.Service.Contracts;

namespace Modules.Service.Application.Abstractions;

public interface IServiceCatalog
{
    Task<IReadOnlyCollection<ServicePriceDto>> GetServicesAsync(
        CancellationToken cancellationToken = default);
}
