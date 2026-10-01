namespace Modules.Chat.Ask;

public class AskChatCommand(ChatRequestDto ChatRequest) : ICommand<ChatResponseDto>
{
    public ChatRequestDto ChatRequest { get; private set; } = ChatRequest;
}
