using Microsoft.Extensions.Options;
using Modules.File.Infrastructure.Options;

namespace Modules.File.Storage;


internal sealed class LocalFileStorage(
    IOptions<FileStorageOptions> options)
    : IFileStorage
{
    private readonly string _rootPath =
        options.Value.RootPath;

    public async Task<string> SaveAsync(
        string key,
        string extension,
        Stream content,
        CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(_rootPath);

        string storagePath =
            $"{key}{extension}";

        string fullPath =
            Path.Combine(
                _rootPath,
                storagePath);

        await using FileStream fileStream =
            new(
                fullPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

        await content.CopyToAsync(
            fileStream,
            cancellationToken);

        return storagePath;
    }

    public Task<Stream> GetAsync(
        string storagePath,
        CancellationToken cancellationToken)
    {
        string fullPath =
            Path.Combine(
                _rootPath,
                storagePath);

        if (!System.IO.File.Exists(fullPath))
        {
            throw new FileNotFoundException(
                "A fájl nem található.",
                fullPath);
        }

        Stream stream =
            new FileStream(
                fullPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                useAsync: true);

        return Task.FromResult(stream);
    }

    public Task DeleteAsync(
        string storagePath,
        CancellationToken cancellationToken)
    {
        string fullPath =
            Path.Combine(
                _rootPath,
                storagePath);

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
