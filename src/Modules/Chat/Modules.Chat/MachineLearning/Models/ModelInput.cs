namespace Modules.Chat.MachineLearning.Models;

internal sealed class ModelInput
{
    public string Text { get; set; } = string.Empty;

    public string Intent { get; set; } = string.Empty;
}
