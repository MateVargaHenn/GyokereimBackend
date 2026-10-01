namespace Modules.Chat.MachineLearning.Models;

internal sealed class ModelOutput
{
    [ColumnName("PredictedLabel")]
    public string PredictedIntent { get; set; } = string.Empty;

    [ColumnName("Score")]
    public float[] Score { get; set; } = [];
}
