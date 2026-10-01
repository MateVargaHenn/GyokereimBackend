namespace Modules.Chat.MachineLearning.Training;

internal interface IIntentModelTrainer
{
    Task TrainAsync(
        string modelPath, 
        CancellationToken cancellationToken = default);
}
