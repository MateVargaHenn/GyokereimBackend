using Modules.Chat.Application;
using Modules.Chat.Models;
using Modules.Service.Application.Abstractions;
using Modules.Service.Contracts;

namespace Modules.Chat.Infrastructure.Providers;

internal sealed class ServiceInformationProvider(
    IServiceCatalog serviceCatalog)
    : IServiceInformationProvider
{
    public async Task<IReadOnlyCollection<ServiceInformation>> GetServicesAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<ServicePriceDto> services =
            await serviceCatalog.GetServicesAsync(cancellationToken);

        return services
            .Select(service => new ServiceInformation(
                service.Id,
                service.Slug,
                service.Name,
                service.ShortDescription,
                service.Description,
                service.Price))
            .ToList();
    }
}
