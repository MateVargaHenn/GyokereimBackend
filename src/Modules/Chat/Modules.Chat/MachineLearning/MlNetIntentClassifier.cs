namespace Modules.Chat.MachineLearning;

internal sealed class MlNetIntentClassifier(
    PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool) 
    : IIntentClassifier
{
    public IntentPrediction Predict(string text)
    {
        var input = new ModelInput
        {
            Text = text
        };

        ModelOutput output = predictionEnginePool.Predict(
            modelName: ChatModel.Name,
            example: input);

        float confidence = output.Score.Length > 0
            ? output.Score.Max()
            : 0f;

        return new IntentPrediction(
            output.PredictedIntent,
            output.Score,
            confidence);
    }
}
