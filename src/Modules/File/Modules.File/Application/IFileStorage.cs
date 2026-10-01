namespace Modules.File.Application;

internal interface IFileStorage
{
    Task<string> SaveAsync(
        string key,
        string extension,
        Stream content,
        CancellationToken cancellationToken);

    Task<Stream> GetAsync(
        string storagePath,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string storagePath,
        CancellationToken cancellationToken);
}
