using Modules.Chat.Models;

namespace Modules.Chat.Application;

internal interface IServiceInformationProvider
{
    Task<IReadOnlyCollection<ServiceInformation>> GetServicesAsync(
        CancellationToken cancellationToken = default);
}

