namespace Modules.Chat.Intents.Domain;

public sealed class Intent : Entity
{
    private readonly List<Answer> _answers = [];
    private readonly List<Utterance> _utterances = [];

    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;

    public IReadOnlyCollection<Answer> Answers => _answers;
    public IReadOnlyCollection<Utterance> Utterances => _utterances;

    public void AddAnswer(string text)
    {
        if (_answers.Any(x =>
            x.Text.Equals(
                text,
                StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        _answers.Add(
            new Answer
            {
                Text = text
            });
    }

    public void AddUtterance(string text)
    {
        if (_utterances.Any(x =>
            x.Text.Equals(
                text,
                StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        _utterances.Add(
            new Utterance
            {
                Text = text
            });
    }
}
