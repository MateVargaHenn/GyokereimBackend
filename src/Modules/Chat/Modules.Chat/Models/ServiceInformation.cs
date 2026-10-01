namespace Modules.Chat.Models;

internal sealed record ServiceInformation(
    int Id,
    string Key,
    string Name,
    string ShortDescription,
    string Description,
    decimal Price);
