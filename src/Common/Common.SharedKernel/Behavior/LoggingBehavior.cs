namespace Common.SharedKernel.Behavior;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(
                "Handling request {RequestName}",
                requestName);
        }

        var stopwatch = Stopwatch.StartNew();

        TResponse response = await next(cancellationToken);

        stopwatch.Stop();

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Handled request {RequestName} in {ElapsedMilliseconds} ms",
                requestName,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
