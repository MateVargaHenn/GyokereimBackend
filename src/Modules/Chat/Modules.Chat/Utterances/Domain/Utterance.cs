namespace Modules.Chat.Utterances.Domain;

public sealed class Utterance : Entity
{
    public string Text { get; set; } = default!;
    public int IntentId { get; set; }
    public Intent Intent { get; set; } = default!;
}
