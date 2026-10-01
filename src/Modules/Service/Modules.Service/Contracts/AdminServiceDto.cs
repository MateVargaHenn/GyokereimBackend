namespace Modules.Service.Contracts;


public sealed record AdminServiceDto(
    int Id,
    string Name,
    string Slug,
    string ShortDescription,
    string Description,
    decimal Price,
    int OrderNumber,
    ServiceStatusCode ServiceStatusCode);
