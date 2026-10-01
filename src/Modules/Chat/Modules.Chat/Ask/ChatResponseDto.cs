namespace Modules.Chat.Ask;

public record ChatResponseDto(
    string MyQuestion, 
    ChatAnswerDto ChatAnswer, 
    string Intent,
    IReadOnlyCollection<int> ReferencedServiceIds);
