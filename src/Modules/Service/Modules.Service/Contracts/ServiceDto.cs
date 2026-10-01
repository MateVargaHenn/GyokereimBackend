namespace Modules.Service.Contracts;

public record ServiceDto(string Name, string Slug, string ShortDescription, string Description, decimal Price)
{
}
