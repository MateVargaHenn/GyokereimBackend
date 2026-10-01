namespace Modules.Service.Domain;

internal class ServiceStatus : EnumEntity<ServiceStatusCode>
{
    internal IReadOnlyCollection<Service> Services { get; } = [];
}
