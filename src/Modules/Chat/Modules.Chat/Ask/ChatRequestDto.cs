namespace Modules.Chat.Ask;

public record ChatRequestDto(
    string Message, 
    string? PreviousIntent,
    IReadOnlyCollection<int>? ReferencedServiceIds);
