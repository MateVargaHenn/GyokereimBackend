namespace Modules.Chat.MachineLearning;

public interface IIntentClassifier
{
    IntentPrediction Predict(string text);
}
