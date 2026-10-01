namespace Modules.Service.Contracts;

public sealed record CreateServiceDto(
    string Name,
    string ShortDescription,
    string Description,
    string Slug,
    decimal Price,
    int OrderNumber,
    ServiceStatusCode ServiceStatusCode);
