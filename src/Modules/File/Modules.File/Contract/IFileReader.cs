namespace Modules.File.Contract;

public interface IFileReader
{
    Task<FileData?> GetFileAsync(
    int id,
    CancellationToken cancellationToken = default);

    Task<FileData?> GetFileAsync(
        string key,
        CancellationToken cancellationToken = default);
}
