namespace Modules.Service.Contracts;

public sealed record ServicePriceDto(
    int Id,
    string Slug,
    string Name,
    string ShortDescription,
    string Description,
    decimal Price);
