namespace Modules.File.Infrastructure.Options;

internal sealed class FileStorageOptions
{
    internal const string SectionName = "FileStorage";

    public string RootPath { get; init; } = default!;
}
