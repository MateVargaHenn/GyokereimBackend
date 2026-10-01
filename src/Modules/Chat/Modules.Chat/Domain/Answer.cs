namespace Modules.Chat.Domain;

public sealed class Answer : Entity
{
    public string Text { get; set; } = null!;
    public int IntentId { get; set; }
    public Intent Intent { get; set; } = null!;
}
