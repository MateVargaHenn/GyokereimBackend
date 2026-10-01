namespace Common.SharedKernel.Logger;

internal static partial class LogMessages
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Handling request {RequestName}")]
    internal static partial void HandlingRequest(
        ILogger logger,
        string RequestName);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Handled request {RequestName} in {ElapsedMilliseconds} ms")]
    internal static partial void HandledRequest(
        ILogger logger,
        string RequestName,
        long ElapsedMilliseconds);
}
