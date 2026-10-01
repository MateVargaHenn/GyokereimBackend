namespace Modules.Chat.MachineLearning;

public sealed record IntentPrediction(
    string Intent,
    float[] Scores,
    float Confidence);
