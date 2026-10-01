using Microsoft.ML;
using Microsoft.ML.Transforms;

namespace Modules.Chat.MachineLearning.Training;

internal sealed class IntentModelTrainer(
    ChatDbContext dbContext)
    : IIntentModelTrainer
{
    public async Task TrainAsync(
        string modelPath,
        CancellationToken cancellationToken = default)
    {
        List<ModelInput> trainingData = await dbContext.Utterances
            .AsNoTracking()
            .Select(utterance => new ModelInput
            {
                Text = utterance.Text,
                Intent = utterance.Intent.Key
            })
            .ToListAsync(cancellationToken);

        if (trainingData.Count == 0)
        {
            throw new InvalidOperationException(
                "Nincs rendelkezésre álló tanítóadat.");
        }

        var mlContext = new MLContext(seed: 42);

        IDataView dataView =
            mlContext.Data.LoadFromEnumerable(trainingData);

        EstimatorChain<KeyToValueMappingTransformer> estimator =
            mlContext.Transforms.Conversion
                .MapValueToKey(
                    outputColumnName: "Label",
                    inputColumnName: nameof(ModelInput.Intent))

            .Append(
                mlContext.Transforms.Text.FeaturizeText(
                    outputColumnName: "Features",
                    inputColumnName: nameof(ModelInput.Text)))

            .Append(
                mlContext.MulticlassClassification.Trainers
                    .SdcaMaximumEntropy(
                        labelColumnName: "Label",
                        featureColumnName: "Features"))

            .Append(
                mlContext.Transforms.Conversion.MapKeyToValue(
                    outputColumnName: "PredictedLabel"));

        ITransformer model = estimator.Fit(dataView);

        Directory.CreateDirectory(
            Path.GetDirectoryName(modelPath)!);

        mlContext.Model.Save(
            model,
            dataView.Schema,
            modelPath);
    }
}
